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
    public class ModuleService : Service<IModuleRepository>, IModuleService
    {
        private readonly IMapper _mapper;

        public ModuleService(IMapper mapper,
                            IMediatorHandler bus, 
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public async Task<ModuleViewModel> GetAsync(Guid id) => 
            _mapper.Map<ModuleViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<ModuleViewModel>> GetAllAsync() => 
            _mapper.Map<IEnumerable<ModuleViewModel>>(await _repository.GetAllAsync());

        public async Task<PaginateResultBase<ModuleViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            var query = await _repository.GetQueryableByCriteria(x => !x.Deleted);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(filter.Search) 
                                        || x.Description.ToLower().Contains(filter.Search));
            }

            var total = query.Count();

            query = query.OrderByDescending(x => x.Name);

            if (int.TryParse(filter.Skip, out int skip))
                query = query.Skip(skip);

            if (int.TryParse(filter.Take, out int take))
                query = query.Take(take);

            var list = _mapper.Map<List<ModuleViewModel>>(query.ToList());

            return new PaginateResultBase<ModuleViewModel>() { Results = list, Total = total };
        }

        public async Task AddAsync(ModuleViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var module = _mapper.Map<Module>(viewModel);
            await _repository.AddAsync(module);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }
        
        public async Task UpdateAsync(ModuleViewModel viewModel)
        {
            var module = await _repository.GetAsync(viewModel.Id);

            if (module != null)
            {
                _mapper.Map(viewModel, module);

                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Título não encontrado"));
            }
        }

        public async Task RemoveAsync(ModuleViewModel viewModel)
        {
            var module = await _repository.GetAsync(viewModel.Id);
            module.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task RestoreAsync(ModuleViewModel viewModel)
        {
            var module = await _repository.GetAsync(viewModel.Id);
            module.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        
    }
}
