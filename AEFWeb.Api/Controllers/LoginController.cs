using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Security;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
            _userService = userService;

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<object> Login(
            [FromBody] LoginViewModel loginViewModel,
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfigurations tokenConfigurations)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(loginViewModel);
            }

            var user = await _userService.GetByEmail(loginViewModel.Email);

            if (user == null) return new { authenticated = false, message = "Usuário não encontrado" };

            if (!user.IsVerified) return new { authenticated = false, message = "Confirme seu e-mail" };

            else if (!_userService.IsVerifyPassword(loginViewModel.Password, user.Password)) return new { authenticated = false, message = "Usuário ou senha incorretos!" };

            else
            {
                var login = new ManageToken().GetLoginObject(tokenConfigurations, signingConfigurations, user);
                if (login == null) return new { authenticated = false, message = "Ocorreu algum erro, tente novamente." };
                return login;
            }
        }
    }
}