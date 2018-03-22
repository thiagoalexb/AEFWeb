using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
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
        public async Task<IActionResult> Get() => Ok(await _userService.GetAllAsync());

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var user = await _userService.GetAsync(id);
            if (user == null) return NotFound();
            return Response(user);
        }

        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _userService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]UserViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _userService.AddAsync(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]UserViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _userService.UpdateAsync(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]UserViewModel entity)
        {
            await _userService.RemoveAsync(entity);

            return Response();
        }

        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]UserViewModel entity)
        {
            await _userService.RestoreAsync(entity);

            return Response();
        }

        [HttpGet]
        [Route("verify-password")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPassword(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null) return Response(new { message = $"Este email: {email} não está cadastrado no nosso banco de dados" });
            return Response(new { userId = user.Id, isVerified = user.IsVerified });
        }

        [HttpPost]
        [Route("update-password")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromBody]UserUpdatePasswordViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            var user = await _userService.UpdatePasswordAsync(entity);
            return Response(user);
        }

        [HttpGet]
        [Route("recover-password")]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword(string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if (user == null) return Response(new { message = $"Este email: {email} não está cadastrado no nosso banco de dados" });
            await _userService.SendRecoverPasswordAsync(user.Id);
            return Response();
        }


        [HttpGet]
        [Route("verify-password-token")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyPasswordToken(Guid token, string email)
        {
            return Response(await _userService.VerifyPasswordToken(token, email));
        }
    }
}
