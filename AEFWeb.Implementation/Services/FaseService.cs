using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AEFWeb.Core.Services.Core;
using System.Linq;

namespace AEFWeb.Implementation.Services
{
    public class FaseService : Service<IFaseRepository>, IFaseService
    {
        private readonly IMapper _mapper;

        public FaseService(IMapper mapper,
                            IMediatorHandler bus, 
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public async Task<FaseViewModel> GetAsync(Guid id) => 
            _mapper.Map<FaseViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<FaseViewModel>> GetAllAsync() => 
            _mapper.Map<IEnumerable<FaseViewModel>>(await _repository.GetAllAsync());

        public async Task<PaginateResultBase<FaseViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            var query = await _repository.GetQueryableByCriteria(x => !x.Deleted);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(filter.Search));
            }

            var total = query.Count();

            query = query.OrderByDescending(x => x.Name);

            if (int.TryParse(filter.Skip, out int skip))
                query = query.Skip(skip);

            if (int.TryParse(filter.Take, out int take))
                query = query.Take(take);

            var list = _mapper.Map<List<FaseViewModel>>(query.ToList());

            return new PaginateResultBase<FaseViewModel>() { Results = list, Total = total };
        }

        public async Task AddAsync(FaseViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var fase = _mapper.Map<Fase>(viewModel);
            await _repository.AddAsync(fase);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }
        
        public async Task UpdateAsync(FaseViewModel viewModel)
        {
            var fase = await _repository.GetAsync(viewModel.Id);

            if (fase != null)
            {
                _mapper.Map(viewModel, fase);

                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Título não encontrado"));
            }
        }

        public async Task RemoveAsync(FaseViewModel viewModel)
        {
            var fase = await _repository.GetAsync(viewModel.Id);
            fase.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task RestoreAsync(FaseViewModel viewModel)
        {
            var fase = await _repository.GetAsync(viewModel.Id);
            fase.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        
    }
}
