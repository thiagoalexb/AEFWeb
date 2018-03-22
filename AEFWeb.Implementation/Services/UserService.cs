using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.EmailSettings;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Services
{
    public class UserService : Service<IUserRepository>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public BaseUrl _urlSettings { get; }

        public UserService(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IMediatorHandler bus,
                            IEmailService emailService,
                            IOptions<BaseUrl> urlSettings) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _emailService = emailService;
            _urlSettings = urlSettings.Value;
        }

        public async Task<UserViewModel> GetAsync(Guid id) =>
            _mapper.Map<UserViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<UserViewModel>> GetAllAsync() =>
            _mapper.Map<IEnumerable<UserViewModel>>(await _repository.GetAllAsync());

        public async Task<PaginateResultBase<UserViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            var query = await _repository.GetQueryableByCriteria(x => !x.Deleted);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(x => x.FirstName.ToLower().Contains(filter.Search) || x.LastName.ToLower().Contains(filter.Search) || x.Email.ToLower().Contains(filter.Search));
            }

            var total = query.Count();

            query = query.OrderByDescending(x => x.FirstName);

            if (int.TryParse(filter.Skip, out int skip))
                query = query.Skip(skip);

            if (int.TryParse(filter.Take, out int take))
                query = query.Take(take);

            var list = _mapper.Map<List<UserViewModel>>(query.ToList());

            return new PaginateResultBase<UserViewModel>() { Results = list, Total = total };
        }

        public async Task<UserUpdatePasswordViewModel> GetByEmailAsync(string email) =>
            _mapper.Map<UserUpdatePasswordViewModel>(await _repository.GetByCriteriaAsync(x => x.Email.ToLower() == email.ToLower()));

        public async Task AddAsync(UserViewModel viewModel)
        {
            var userdb = await _repository.GetByCriteriaAsync(x => x.Email.ToLower() == viewModel.Email.ToLower());
            if (userdb != null && userdb.Deleted == false)
            {
                await _bus.RaiseEvent(new Notification("Email", "Este e-mail já está sendo usado"));
                return;
            }
            else if (userdb != null)
            {
                _mapper.Map(viewModel, userdb);
                viewModel.Id = userdb.Id;
            }
            else
            {
                viewModel.Id = Guid.NewGuid();
                var user = _mapper.Map<User>(viewModel);
                await _repository.AddAsync(user);
            }


            if (await Commit())
            {
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));

                await _emailService.SendEmailAsync(viewModel.Email, "Cadastro AEF", BuildActiveMessage(viewModel));
            }
        }

        public async Task UpdateAsync(UserViewModel viewModel)
        {
            var user = await _repository.GetAsync(viewModel.Id);

            if (user != null)
            {
                if (user.Email != viewModel.Email)
                {
                    await _bus.RaiseEvent(new Notification("Email", "Não é possível alterar e-mail."));
                    return;
                }

                _mapper.Map(viewModel, user);
                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(user), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Usuário não encontrado"));
            }
        }

        public async Task<UserViewModel> UpdatePasswordAsync(UserUpdatePasswordViewModel viewModel)
        {
            var user = await _repository.GetAsync(viewModel.Id);

            if (user != null)
            {
                if (!user.IsVerified || user.PasswordChangeToken != null)
                {
                    user.SetPassword(Utils.Utils.EncryptPassword(viewModel.Password));
                    user.SetPasswordChangeToken(null);
                    await Commit();
                    return _mapper.Map<UserViewModel>(user);
                }
                else
                {
                    await _bus.RaiseEvent(new Notification("defaultError", "Sua conta já esta ativa, faça o login."));
                    return null;
                }
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Usuário não encontrato."));
                return null;
            }
        }

        public async Task RemoveAsync(UserViewModel viewModel)
        {
            var user = await _repository.GetAsync(viewModel.Id);
            user.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task RestoreAsync(UserViewModel viewModel)
        {
            var user = await _repository.GetAsync(viewModel.Id);
            user.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        public bool IsVerifyPassword(string passwordLogin, string passwordUser) =>
            Utils.Utils.EncryptPassword(passwordLogin).ToLower() == passwordUser.ToLower();

        public async Task SendRecoverPasswordAsync(Guid id)
        {
            var user = await _repository.GetByCriteriaAsync(x => x.Id == id);
            user.SetPasswordChangeToken(Guid.NewGuid());

            if (await Commit())
                await _emailService.SendEmailAsync(user.Email, "Recuperação de senha AEF", BuildRecoverPasswordMessage(user));
        }

        public async Task<UserViewModel> GetByPasswordTokenAsync(Guid token, string email)
        {
            return _mapper.Map<UserViewModel>(await _repository.GetByCriteriaAsync(x => x.Email == email && x.PasswordChangeToken == token));
        }

        public async Task<object> VerifyPasswordToken(Guid token, string email)
        {
            if (token == Guid.Empty)
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Digito verificador inválido"));
                return null;
            }
            if (string.IsNullOrEmpty(email))
            {
                await _bus.RaiseEvent(new Notification("defaultError", "É obrigatório um email para verificar"));
                return null;
            }
            var userByEmail = await GetByEmailAsync(email);
            if (userByEmail == null)
            {
                await _bus.RaiseEvent(new Notification("defaultError", $"Este email: {email} não está cadastrado no nosso banco de dados"));
                return null;
            }

            var user = await GetByPasswordTokenAsync(token, email);
            if (user == null)
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Não há nenhum pedido de mudança de senha para este email"));
                return null;
            }
            return new { userId = user.Id };
        }

        private string BuildActiveMessage(UserViewModel viewModel)
        {
            var html = Properties.Resources.ConfirmEmailTemplate;
            html = html.Replace("{Url}", $"{_urlSettings.Url}#/usuario/ativar-conta/{viewModel.Email}");
            return html;
        }

        private string BuildRecoverPasswordMessage(User user)
        {
            var html = Properties.Resources.RecoverPasswordTemplate;
            html = html.Replace("{Url}", $"{_urlSettings.Url}#/usuario/recuperar-senha/{user.Email}/{user.PasswordChangeToken}");
            return html;
        }


    }
}