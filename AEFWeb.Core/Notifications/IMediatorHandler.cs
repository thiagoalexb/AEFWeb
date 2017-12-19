using System.Threading.Tasks;
using MediatR;
using AEFWeb.Data.Entities;

namespace AEFWeb.Core.Notifications
{
    public interface IMediatorHandler
    {
        Task RaiseEvent<T>(T @event) where T : INotification;
        Task RaiseEventLog<T>(T @event) where T : EventLog;
    }
}
