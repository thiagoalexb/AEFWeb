using AEFWeb.Core.Services.Core;
using AEFWeb.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Core.Services
{
    public interface IUserService : IService<UserViewModel>
    {
        Task<UserViewModel> UpdatePasswordAsync(UserUpdatePasswordViewModel viewModel);
        bool IsVerifyPassword(string passwordLogin, string passwordUser);
        Task<UserUpdatePasswordViewModel> GetByEmailAsync(string email);
        Task SendRecoverPasswordAsync(Guid id);
        Task<UserViewModel> GetByPasswordTokenAsync(Guid token, string email);
        Task<object> VerifyPasswordToken(Guid token, string email);
    }
}
