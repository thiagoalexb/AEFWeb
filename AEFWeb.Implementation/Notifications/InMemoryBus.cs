using AEFWeb.Core.Notifications;
using AEFWeb.Core.Services;
using AEFWeb.Data.Entities;
using MediatR;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Notifications
{
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventLogService _eventLogService;

        public InMemoryBus(IMediator mediator, IEventLogService eventLogService)
        {
            _mediator = mediator;
            _eventLogService = eventLogService;
        }

        public Task RaiseEvent<T>(T @event) where T : INotification
        {          
            return Publish(@event);
        }

        public Task RaiseEventLog<T>(T @event) where T : EventLog
        {
            _eventLogService.Add(@event);
            return Task.FromResult("Log");
        }

        private Task Publish<T>(T message) where T : INotification =>
            _mediator.Publish(message);
    }
}
