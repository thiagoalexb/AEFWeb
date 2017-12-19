using AEFWeb.Core.Services.Core;
using AEFWeb.Core.ViewModels;

namespace AEFWeb.Core.Services
{
    public interface IUserService : IService<UserViewModel>
    {
        void UpdatePassword(UserUpdatePasswordViewModel viewModel);
        bool IsVerifyPassword(string passwordLogin, string passwordUser);
        UserUpdatePasswordViewModel GetByEmail(string email);
    }
}
