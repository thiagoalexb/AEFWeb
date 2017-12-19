using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Services.Core;

namespace AEFWeb.Implementation.Services
{
    public class ErrorLogService : Service<IErrorLogRepository>, IErrorLogService
    {
        public ErrorLogService(IMediatorHandler bus, IUnitOfWork unitOfWork) 
                                : base(bus, unitOfWork) { }
        
        public void Add(ErrorLog entity)
        {
            _repository.Add(entity);
            _unitOfWork.Complete();
        }
    }
}
