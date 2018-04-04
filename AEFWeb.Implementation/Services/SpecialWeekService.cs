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
    public class SpecialWeekService : Service<ISpecialWeekRepository>, ISpecialWeekService
    {
        private readonly IMapper _mapper;

        public SpecialWeekService(IMapper mapper,
                            IMediatorHandler bus, 
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public async Task<SpecialWeekViewModel> GetAsync(Guid id) => 
            _mapper.Map<SpecialWeekViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<SpecialWeekViewModel>> GetAllAsync() => 
            _mapper.Map<IEnumerable<SpecialWeekViewModel>>(await _repository.GetAllAsync());

        public async Task<PaginateResultBase<SpecialWeekViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            var query = await _repository.GetQueryableByCriteria(x => !x.Deleted);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(x => x.Title.ToLower().Contains(filter.Search) 
                                        || x.Description.ToLower().Contains(filter.Search));
            }

            var total = query.Count();

            query = query.OrderByDescending(x => x.Title);

            if (int.TryParse(filter.Skip, out int skip))
                query = query.Skip(skip);

            if (int.TryParse(filter.Take, out int take))
                query = query.Take(take);

            var list = _mapper.Map<List<SpecialWeekViewModel>>(query.ToList());

            return new PaginateResultBase<SpecialWeekViewModel>() { Results = list, Total = total };
        }

        public async Task AddAsync(SpecialWeekViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var specialWeek = _mapper.Map<SpecialWeek>(viewModel);
            await _repository.AddAsync(specialWeek);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }
        
        public async Task UpdateAsync(SpecialWeekViewModel viewModel)
        {
            var specialWeek = await _repository.GetAsync(viewModel.Id);

            if (specialWeek != null)
            {
                _mapper.Map(viewModel, specialWeek);

                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Título não encontrado"));
            }
        }

        public async Task RemoveAsync(SpecialWeekViewModel viewModel)
        {
            var specialWeek = await _repository.GetAsync(viewModel.Id);
            specialWeek.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task RestoreAsync(SpecialWeekViewModel viewModel)
        {
            var specialWeek = await _repository.GetAsync(viewModel.Id);
            specialWeek.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        public async Task<List<AutoCompleteViewModel>> GetAutoCompleteAsync(string search)
        {
            search = search.ToLower();
            var query = await _repository.GetQueryableByCriteria(x => x.Title.Contains(search));

            return query.Select(x => new AutoCompleteViewModel()
            {
                Id = x.Id,
                Label = x.Title
            })
            .ToList();
        }
    }
}
