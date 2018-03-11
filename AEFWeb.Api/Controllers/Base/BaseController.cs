using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;

namespace AEFWeb.Api.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        private readonly NotificationService _notifications;

        protected BaseController(INotificationHandler<Notification> notifications)
        {
            _notifications = (NotificationService)notifications;
        }

        protected bool IsValidOperation() => (!_notifications.HasNotifications());

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            
            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().ToDictionary(s => s.Source, s => s.Message)
        });
        }

        protected void NotifyModelStateErrors()
        {
            var values = ModelState.Values.ToList();
            foreach (var value in values)
            {
                var error = value.Errors.FirstOrDefault();
                var erroMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                var key = value.GetType().GetProperty("Key")?.GetValue(value)?.ToString();
                NotifyError(string.Empty, key, erroMsg);
            }
        }

        protected void NotifyError(string code, string source, string message)
        {
            _notifications.Handle(new Notification(source, message), new CancellationToken());
        }
    }
}
