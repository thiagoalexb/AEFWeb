using AEFWeb.Core.Notifications;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Services.Core
{
    public abstract class Service<TRepository> where TRepository : class
    {
        protected IMediatorHandler _bus;
        protected IUnitOfWork _unitOfWork;
        protected TRepository _repository;
        protected string Type { get => this.GetType()?.Name?.Replace("Service", ""); }

        public Service(IMediatorHandler bus,
                        IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<TRepository>();
        }

        protected async Task<bool> Commit()
        {
            if (! await _unitOfWork.Complete())
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Erro ao salvar, tente novamente."));
                return false;
            }
            return true;
        }

        public async Task RegisterLog(EventLog log)
        {
            await _bus.RaiseEventLog(log);
        }
    }
}
