using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;

namespace AEFWeb.Implementation.Services
{
    public class EventLogService : IEventLogService
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EventLogService(IUnitOfWork unitOfWork, IEventLogRepository eventLogRepository)
        {
            _unitOfWork = unitOfWork;
            _eventLogRepository = eventLogRepository;
        }

        public void Add(EventLog entity)
        {
            _eventLogRepository.Add(entity);
            _unitOfWork.Complete();
        }
    }
}
