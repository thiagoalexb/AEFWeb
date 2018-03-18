using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.EmailSettings;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task<UserViewModel> Get(Guid id) =>
            _mapper.Map<UserViewModel>(await _repository.Get(id));

        public async Task<IEnumerable<UserViewModel>> GetAll() =>
            _mapper.Map<IEnumerable<UserViewModel>>(await _repository.GetAll());

        public async Task<UserUpdatePasswordViewModel> GetByEmail(string email) =>
            _mapper.Map<UserUpdatePasswordViewModel>(await _repository.GetByCriteria(x => x.Email.ToLower() == email.ToLower()));

        public async Task Add(UserViewModel viewModel)
        {
            var userdb = await _repository.GetByCriteria(x => x.Email.ToLower() == viewModel.Email.ToLower());
            if(userdb != null && userdb.Deleted == false)
            {
                await _bus.RaiseEvent(new Notification("Email", "Este e-mail já está sendo usado"));
                return;
            }
            else if(userdb != null)
            {
                _mapper.Map(viewModel, userdb);
                viewModel.Id = userdb.Id;
            }
            else
            {
                viewModel.Id = Guid.NewGuid();
                var user = _mapper.Map<User>(viewModel);
                await _repository.Add(user);
            }
            

            if (await Commit())
            {
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));

                await _emailService.SendEmailAsync(viewModel.Email, "Cadastro AEF", BuildActiveMessage(viewModel));
            }
        }

        public async Task Update(UserViewModel viewModel)
        {
            var user = await _repository.Get(viewModel.Id);

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

        public async Task<UserViewModel> UpdatePassword(UserUpdatePasswordViewModel viewModel)
        {
            var user = await _repository.Get(viewModel.Id);
            
            if(user != null)
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

        public async Task Remove(UserViewModel viewModel)
        {
            var user = await _repository.Get(viewModel.Id);
            user.SetDeleted(true); 

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task Restore(UserViewModel viewModel)
        {
            var user = await _repository.Get(viewModel.Id);
            user.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        public bool IsVerifyPassword(string passwordLogin, string passwordUser) =>
            Utils.Utils.EncryptPassword(passwordLogin).ToLower() == passwordUser.ToLower();

        public async Task SendRecoverPassword(Guid id)
        {
            var user = await _repository.GetByCriteria(x => x.Id == id);
            user.SetPasswordChangeToken(Guid.NewGuid());

            if (await Commit())
                await _emailService.SendEmailAsync(user.Email, "Recuperação de senha AEF", BuildRecoverPasswordMessage(user));
        }

        public async Task<UserViewModel> GetByPasswordToken(Guid token, string email)
        {
            return _mapper.Map<UserViewModel>(await _repository.GetByCriteria(x => x.Email == email && x.PasswordChangeToken == token));
        }

        private string BuildActiveMessage(UserViewModel viewModel)
        {
            var html = Properties.Resources.ConfirmEmailTemplate;
            html = html.Replace("{Url}", $"{_urlSettings.Url}ativar-conta/{viewModel.Email}");
            return html;
        }

        private string BuildRecoverPasswordMessage(User user)
        {
            var html = Properties.Resources.RecoverPasswordTemplate;
            html = html.Replace("{Url}", $"{_urlSettings.Url}recuperar-senha/{user.Email}/{user.PasswordChangeToken}");
            return html;
        }
    }
}