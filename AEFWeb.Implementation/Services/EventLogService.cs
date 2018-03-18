using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using System.Threading.Tasks;

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

        public async Task Add(EventLog entity)
        {
            await _eventLogRepository.Add(entity);
            await _unitOfWork.Complete();
        }
    }
}
