using AEFWeb.Core.Notifications;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using System;

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

        protected bool Commit()
        {
            if (!_unitOfWork.Complete())
            {
                _bus.RaiseEvent(new Notification("Erro ao salvar, tente novamente."));
                return false;
            }
            return true;
        }

        public void RegisterLog(EventLog log)
        {
            _bus.RaiseEventLog(log);
        }
    }
}
