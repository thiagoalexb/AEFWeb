using AEFWeb.Core.Services.Core;
using AEFWeb.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Core.Services
{
    public interface IUserService : IService<UserViewModel>
    {
        Task<UserViewModel> UpdatePassword(UserUpdatePasswordViewModel viewModel);
        bool IsVerifyPassword(string passwordLogin, string passwordUser);
        Task<UserUpdatePasswordViewModel> GetByEmail(string email);
        Task SendRecoverPassword(Guid id);
        Task<UserViewModel> GetByPasswordToken(Guid token, string email);
    }
}
