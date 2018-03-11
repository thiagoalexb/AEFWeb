using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    //[Authorize("Bearer")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _userService = userService;

        [HttpGet]
        [Route("get-all")]
        public IActionResult Get() => Ok(_userService.GetAll());

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var user = _userService.Get(id);
            if (user == null) return NotFound();
            return Response(user);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public IActionResult Post([FromBody]UserViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _userService.Add(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public IActionResult Put([FromBody]UserViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _userService.Update(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public IActionResult Delete([FromBody]UserViewModel entity)
        {
            _userService.Remove(entity);

            return Response();
        }

        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public IActionResult Restore([FromBody]UserViewModel entity)
        {
            _userService.Restore(entity);

            return Response();
        }

        [HttpGet]
        [Route("verify-password")]
        [AllowAnonymous]
        public IActionResult VerifyPassword(string email)
        {
            var user = _userService.GetByEmail(email);
            if (user == null) return Response(new { message = $"Este email: {email} não está cadastrado no nosso banco de dados" });
            return Response(new { userId = user.Id, isVerified = user.IsVerified });
        }

        [HttpPost]
        [Route("update-password")]
        [AllowAnonymous]
        public IActionResult UpdatePassword([FromBody]UserUpdatePasswordViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            var user = _userService.UpdatePassword(entity);
            return Response(user);
        }

        [HttpGet]
        [Route("recover-password")]
        [AllowAnonymous]
        public IActionResult RecoverPassword(string email)
        {
            var user = _userService.GetByEmail(email);
            if (user == null) return Response(new { message = $"Este email: {email} não está cadastrado no nosso banco de dados" });
            _userService.SendRecoverPassword(user.Id);
            return Response();
        }


        [HttpGet]
        [Route("verify-password-token")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPasswordToken(Guid token, string email)
        {
            if(token == Guid.Empty)
                return Response(new { message = "Digito verificador inválido" });
            var userByEmail = _userService.GetByEmail(email);
            if (userByEmail == null)
                return Response(new { message = $"Este email: {email} não está cadastrado no nosso banco de dados" });
            var user = await _userService.GetByPasswordToken(token, email);
            if(user == null)
                return Response(new { message = "Não há nenhum pedido de mudança de senha para este email" });
            return Response();
        }
    }
}
