using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Services.Core;

namespace AEFWeb.Implementation.Services
{
    public class EventLogService : Service<IEventLogRepository>, IEventLogService
    {
        public EventLogService(IMediatorHandler bus, IUnitOfWork unitOfWork) 
                                    : base(bus, unitOfWork) { }

        public void Add(EventLog entity)
        {
            _repository.Add(entity);
            _unitOfWork.Complete();
        }
    }
}
