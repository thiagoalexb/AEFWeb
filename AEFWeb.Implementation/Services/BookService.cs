using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AEFWeb.Implementation.Services
{
    public class BookService : Service<IBookRepository>, IBookService
    {
        private readonly IMapper _mapper;

        public BookService(IMapper mapper,
                            IMediatorHandler bus, 
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public async Task<BookViewModel> Get(Guid id) => 
            _mapper.Map<BookViewModel>(await _repository.Get(id));

        public async Task<IEnumerable<BookViewModel>> GetAll() => 
            _mapper.Map<IEnumerable<BookViewModel>>(await _repository.GetAll());

        public async Task Add(BookViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var book = _mapper.Map<Book>(viewModel);
            await _repository.Add(book);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }
        
        public async Task Update(BookViewModel viewModel)
        {
            var book = await _repository.Get(viewModel.Id);

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

        public async Task Remove(BookViewModel viewModel)
        {
            var book = await _repository.Get(viewModel.Id);
            book.SetDeleted(true);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public async Task Restore(BookViewModel viewModel)
        {
            var book = await _repository.Get(viewModel.Id);
            book.SetDeleted(false);

            if (await Commit())
                await RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }
    }
}
