using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;

namespace AEFWeb.Implementation.Services
{
    public class EventLogService : IEventLogService
    {
        private readonly IEventLogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EventLogService(IUnitOfWork unitOfWork, IEventLogRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<IEventLogRepository>();
        }

        public void Add(EventLog entity)
        {
            _repository.Add(entity);
            _unitOfWork.Complete();
        }
    }
}
