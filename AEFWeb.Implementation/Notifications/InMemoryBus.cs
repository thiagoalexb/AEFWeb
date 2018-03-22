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

        public async Task RaiseEvent<T>(T @event) where T : INotification =>
             await Publish(@event);

        public async Task RaiseEventLog<T>(T @event) where T : EventLog =>
            await _eventLogService.AddAsync(@event);

        private async Task Publish<T>(T message) where T : INotification =>
            await _mediator.Publish(message);
    }
}
