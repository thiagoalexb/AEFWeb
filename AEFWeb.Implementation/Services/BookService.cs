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
using System.Linq;

namespace AEFWeb.Implementation.Services
{
    public class BookService : Service<IBookRepository>, IBookService
    {
        private readonly IMapper _mapper;

        public BookService(IMapper mapper,
                            IMediatorHandler bus, 
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public BookViewModel Get(Guid id) => 
            _mapper.Map<BookViewModel>(_repository.Get(id));

        public IEnumerable<BookViewModel> GetAll() => 
            _mapper.Map<IEnumerable<BookViewModel>>(_repository.GetAll());

        public void Add(BookViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid();
            var book = _mapper.Map<Book>(viewModel);
            _repository.Add(book);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        }
        
        public void Update(BookViewModel viewModel)
        {
            var book = _repository.Get(viewModel.Id);

            if (book != null)
            {
                _mapper.Map(viewModel, book);

                if (Commit())
                    RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
            }
            else
            {
                _bus.RaiseEvent(new Notification("defaultError", "Título não encontrado"));
            }
        }

        public void Remove(BookViewModel viewModel)
        {
            var book = _repository.Get(viewModel.Id);
            book.SetDeleted(true);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        }

        public void Restore(BookViewModel viewModel)
        {
            var book = _repository.Get(viewModel.Id);
            book.SetDeleted(false);

            if (Commit())
                RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        }
    }
}
