using MediatR;
using System;

namespace AEFWeb.Implementation.Notifications
{
    public class Notification : INotification
    {
        public string Source { get; private set; }
        public string Message { get; private set; }

        public Notification(string source, string message)
        {
            Source = source;
            Message = message;
        }
    }
}
