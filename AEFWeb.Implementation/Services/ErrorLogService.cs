using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Services.Core;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Services
{
    public class ErrorLogService : Service<IErrorLogRepository>, IErrorLogService
    {
        public ErrorLogService(IMediatorHandler bus, IUnitOfWork unitOfWork) 
                                : base(bus, unitOfWork) { }
        
        public async Task Add(ErrorLog entity)
        {
            await _repository.Add(entity);
            await _unitOfWork.Complete();
        }
    }
}
