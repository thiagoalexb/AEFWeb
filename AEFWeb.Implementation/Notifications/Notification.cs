using MediatR;
using System;

namespace AEFWeb.Implementation.Notifications
{
    public class Notification : INotification
    {
        public string Message { get; private set; }

        public Notification(string message) => 
            Message = message;
    }
}
