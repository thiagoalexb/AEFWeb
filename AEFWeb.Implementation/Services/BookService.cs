﻿using AEFWeb.Core.Notifications;
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
    public class BookService : Service<IBookRepository>, IBookService
    {
        private readonly IMapper _mapper;

        public BookService(IMapper mapper,
                            IMediatorHandler bus, 
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public async Task<BookViewModel> GetAsync(Guid id) => 
            _mapper.Map<BookViewModel>(await _repository.GetAsync(id));

        public async Task<IEnumerable<BookViewModel>> GetAllAsync() => 
            _mapper.Map<IEnumerable<BookViewModel>>(await _repository.GetAllAsync());

        public async Task<PaginateResultBase<BookViewModel>> GetPaginateAsync(PaginateFilterBase filter)
        {
            var query = await _repository.GetQueryableByCriteria(x => !x.Deleted);

            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();
                query = query.Where(x => x.Author.ToLower().Contains(filter.Search) 
                                        || x.Edition.ToLower().Contains(filter.Search)
                                        || x.PublishingCompany.ToLower().Contains(filter.Search)
                                        || x.Title.ToLower().Contains(filter.Search)
                                        || x.Value.ToString().Contains(filter.Search));
            }

            var total = query.Count();

            query = query.OrderByDescending(x => x.Title);

            if (int.TryParse(filter.Skip, out int skip))
                query = query.Skip(skip);

            if (int.TryParse(filter.Take, out int take))
                query = query.Take(take);

            var list = _mapper.Map<List<BookViewModel>>(query.ToList());

            return new PaginateResultBase<BookViewModel>() { Results = list, Total = total };
        }

        public async Task AddAsync(BookViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var book = _mapper.Map<Book>(viewModel);
            await _repository.AddAsync(book);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }
        
        public async Task UpdateAsync(BookViewModel viewModel)
        {
            var book = await _repository.GetAsync(viewModel.Id);

            if (book != null)
            {
                _mapper.Map(viewModel, book);

                if (await Commit())
                    await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                await _bus.RaiseEvent(new Notification("defaultError", "Título não encontrado"));
            }
        }

        public async Task RemoveAsync(BookViewModel viewModel)
        {
            var book = await _repository.GetAsync(viewModel.Id);
            book.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task RestoreAsync(BookViewModel viewModel)
        {
            var book = await _repository.GetAsync(viewModel.Id);
            book.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }

        
    }
}
