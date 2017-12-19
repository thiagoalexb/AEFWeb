using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Notifications
{
    public class NotificationService : INotificationHandler<Notification>
    {
        private List<Notification> _notifications;

        public NotificationService()
        {
            _notifications = new List<Notification>();
        }

        public Task Handle(Notification message, CancellationToken cancellationToken)
        {
            _notifications.Add(message);
            return Task.FromResult("Message");
        }

        public virtual List<Notification> GetNotifications() =>
            _notifications;

        public virtual bool HasNotifications() =>
            GetNotifications().Any();

        public void Dispose()
        {
            _notifications = new List<Notification>();
        }
    }
}
