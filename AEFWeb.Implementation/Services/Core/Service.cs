using AEFWeb.Core.Notifications;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Core.Repositories.Core;
using AEFWeb.Core.ViewModels.Core;
using AEFWeb.Data.Entities.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

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

        public async Task<List<AutoCompleteViewModel>> GetAutoCompleteAsync<TEntity> (
            string search,
            Expression<Func<TEntity, string>> labelField) where TEntity : Entity
        {
            Guid id;
            var repository = _repository as IRepository<TEntity>;

            if (Guid.TryParse(search, out id))
            {
                var entity = await repository.GetAsync(id);

                return new List<AutoCompleteViewModel>
                {
                    new AutoCompleteViewModel
                    {
                        Id = entity.Id,
                        Label = labelField.Compile().Invoke(entity)
                    }
                };
            }
            else
            {
                search = search.ToLower();
                var query = await repository.GetQueryableByCriteria(x =>
                    labelField.Compile().Invoke(x).Contains(search)
                );

                return query.Select(x => new AutoCompleteViewModel()
                {
                    Id = x.Id,
                    Label = labelField.Compile().Invoke(x)
                })
                .ToList();
            }
        }
    }
}
