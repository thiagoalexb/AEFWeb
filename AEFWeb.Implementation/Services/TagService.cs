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
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AEFWeb.Implementation.Services
{
    public class TagService : Service<ITagRepository>, ITagService
    {
        private readonly IMapper _mapper;
        private readonly IPostTagRepository _postTagRepository;

        public TagService(IUnitOfWork unitOfWork,
                        IMapper mapper,
                        IMediatorHandler bus,
                        IPostTagRepository postTagRepository) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _postTagRepository = unitOfWork.Repository<IPostTagRepository>();
        }

        public async Task<TagViewModel> GetAsync(Guid id) => 
            _mapper.Map<TagViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<TagViewModel>> GetAllAsync() => 
            _mapper.Map<IEnumerable<TagViewModel>>(await _repository.GetAllAsync());

        public async Task<PaginateResultBase<TagViewModel>> GetPaginateAsync(PaginateFilterBase filter)
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

            var list = _mapper.Map<List<TagViewModel>>(query.ToList());

            return new PaginateResultBase<TagViewModel>() { Results = list, Total = total };
        }

        public async Task AddAsync(TagViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var tag = _mapper.Map<Tag>(viewModel);
            await _repository.AddAsync(tag);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }

        public async Task UpdateAsync(TagViewModel viewModel)
        {
            var tag = await _repository.GetAsync(viewModel.Id);

            if (tag != null)
            {
                _mapper.Map(viewModel, tag);

                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Tag não encontrada"));
            }
        }

        public async Task RemoveAsync(TagViewModel viewModel)
        {
            var tag = await _repository.GetAsync(viewModel.Id);
            tag.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task RestoreAsync(TagViewModel viewModel)
        {
            var tag = await _repository.GetAsync(viewModel.Id);
            tag.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        public async Task<List<AutoCompleteViewModel>> GetAutoCompleteAsync(string search)
        {
            search = search.ToLower();
            var query = await _repository.GetQueryableByCriteria(x => x.Name.Contains(search));

            return query.Select(x => new AutoCompleteViewModel()
            {
                Id = x.Id,
                Label = x.Name
            })
            .ToList();
        }
    }
}
