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

        public UserViewModel Get(Guid id) =>
            _mapper.Map<UserViewModel>(_repository.Get(id));

        public IEnumerable<UserViewModel> GetAll() =>
            _mapper.Map<IEnumerable<UserViewModel>>(_repository.GetAll());

        public UserUpdatePasswordViewModel GetByEmail(string email) =>
            _mapper.Map<UserUpdatePasswordViewModel>(_repository.GetByCriteria(x => x.Email.ToLower() == email.ToLower()));

        public void Add(UserViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();

            if (_repository.GetByCriteria(x => x.Email.ToLower() == viewModel.Email.ToLower()) != null)
            {
                _bus.RaiseEvent(new Notification("Email", "Este e-mail já está sendo usado"));
                return;
            }

            var user = _mapper.Map<User>(viewModel);
            _repository.Add(user);

            if (Commit())
            {
                RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(user), Type, "Add"));

                _emailService.SendEmailAsync(viewModel.Email, "Cadastro AEF", BuildActiveMessage(viewModel));
            }
        }

        public void Update(UserViewModel viewModel)
        {
            var user = _repository.Get(viewModel.Id);

            if (user != null)
            {
                if (user.Email != viewModel.Email)
                {
                    _bus.RaiseEvent(new Notification("Email", "Não é possível alterar e-mail."));
                    return;
                }

                _mapper.Map(viewModel, user);
                if (Commit())
                    RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(user), Type, "Update"));
            }
            else
            {
                _bus.RaiseEvent(new Notification("defaultError", "Usuário não encontrado"));
            }
        }

        public UserViewModel UpdatePassword(UserUpdatePasswordViewModel viewModel)
        {
            var user = _repository.Get(viewModel.Id);
            
            if(user != null)
            {
                if (!user.IsVerified || user.PasswordChangeToken != null)
                {
                    user.SetPassword(Utils.Utils.EncryptPassword(viewModel.Password));
                    user.SetPasswordChangeToken(null);
                    Commit();
                    return _mapper.Map<UserViewModel>(user);
                }
                else
                {
                    _bus.RaiseEvent(new Notification("defaultError", "Sua conta já esta ativa, faça o login."));
                    return null;
                }
            }
            else
            {
                _bus.RaiseEvent(new Notification("defaultError", "Usuário não encontrato."));
                return null;
            }
        }

        public void Remove(UserViewModel viewModel)
        {
            var user = _repository.Get(viewModel.Id);
            user.SetDeleted(true); 

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public void Restore(UserViewModel viewModel)
        {
            var user = _repository.Get(viewModel.Id);
            user.SetDeleted(false);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        public bool IsVerifyPassword(string passwordLogin, string passwordUser) =>
            Utils.Utils.EncryptPassword(passwordLogin).ToLower() == passwordUser.ToLower();

        public async Task SendRecoverPassword(Guid id)
        {
            var user = _repository.GetByCriteria(x => x.Id == id);
            user.SetPasswordChangeToken(Guid.NewGuid());

            if (Commit())
                await _emailService.SendEmailAsync(user.Email, "Recuperação de senha AEF", BuildRecoverPasswordMessage(user));
        }

        public async Task<UserViewModel> GetByPasswordToken(Guid token, string email)
        {
            return _mapper.Map<UserViewModel>(_repository.GetByCriteria(x => x.Email == email && x.PasswordChangeToken == token));
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