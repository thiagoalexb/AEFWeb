using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Repositories.Core;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AEFWeb.Implementation.Services
{
    public class UserService : Service<IUserRepository>, IUserService
    {
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IMediatorHandler bus) : base(bus, unitOfWork) => _mapper = mapper;

        public UserViewModel Get(Guid id) =>
            _mapper.Map<UserViewModel>(_repository.Get(id));

        public IEnumerable<UserViewModel> GetAll() =>
            _mapper.Map<IEnumerable<UserViewModel>>(_repository.GetAll());

        public UserUpdatePasswordViewModel GetByEmail(string email) =>
            _mapper.Map<UserUpdatePasswordViewModel>(_repository.GetByCriteria(x => x.Email.ToLower() == email.ToLower()));

        public void Add(UserViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();

            if (_repository.Find(x => x.Email.ToLower() == viewModel.Email.ToLower()).Count() > 0)
            {
                _bus.RaiseEvent(new Notification("Este e-mail já está sendo usado"));
                return;
            }

            var user = _mapper.Map<User>(viewModel);
            _repository.Add(user);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(user), Type, "Add"));
        }

        public void Update(UserViewModel viewModel)
        {
            var user = _repository.Get(viewModel.Id);

            if (user != null)
            {
                if (user.Id != viewModel.Id && _repository.Find(x => x.Email.ToLower() == viewModel.Email.ToLower()).Count() > 0)
                {
                    _bus.RaiseEvent(new Notification("Este e-mail já está sendo usado"));
                    return;
                }

                _mapper.Map(viewModel, user);
                if (Commit())
                    RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(user), Type, "Update"));
            }
            else
            {
                _bus.RaiseEvent(new Notification("Usuário não encontrado"));
            }
        }

        public void UpdatePassword(UserUpdatePasswordViewModel viewModel)
        {
            var user = _repository.Get(viewModel.Id);

            if (user != null && !user.IsVerified)
            {
                user.SetPassword(Utils.Utils.EncryptPassword(viewModel.Password));
            }

            Commit();
        }

        public void Remove(UserViewModel viewModel)
        {
            _repository.Remove(_repository.Get(viewModel.Id));
            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public bool IsVerifyPassword(string passwordLogin, string passwordUser) => 
            Utils.Utils.EncryptPassword(passwordLogin).ToLower() == passwordUser;
    }
}
