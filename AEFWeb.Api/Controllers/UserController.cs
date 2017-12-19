using AEFWeb.Api.Controllers.Base;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
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
        public IActionResult Delete([FromBody]UserViewModel entity)
        {
            _userService.Remove(entity);

            return Response();
        }

        [HttpGet]
        [Route("verify-password")]
        public IActionResult VerifyPassword(string email)
        {
            var user = _userService.GetByEmail(email);
            if (user == null) return Response(new { message = $"Este email: {email} não está cadastrado no nosso banco de dados" });
            return Response(new { userId = user.Id, isVerified = user.IsVerified });
        }

        [HttpPost]
        [Route("update-password")]
        public IActionResult UpdatePassword([FromBody]UserUpdatePasswordViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _userService.UpdatePassword(entity);
            return Response(entity);
        }
    }
}
