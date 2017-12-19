using AEFWeb.Core.Notifications;
using AEFWeb.Core.Repositories;
using AEFWeb.Core.Services;
using AEFWeb.Core.UnitOfWork;
using AEFWeb.Implementation.Services.Core;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Data.Entities;

namespace AEFWeb.Implementation.Services
{
    public class LessonService : Service<ILessonRepository>, ILessonService
    {
        private readonly IMapper _mapper;

        public LessonService(IMapper mapper,
                            IMediatorHandler bus,
                            IUnitOfWork unitOfWork) : base(bus, unitOfWork) => _mapper = mapper;

        public LessonViewModel Get(Guid id) => 
            _mapper.Map<LessonViewModel>(_repository.Get(id));

        public IEnumerable<LessonViewModel> GetAll() => 
            _mapper.Map<IEnumerable<LessonViewModel>>(_repository.GetAll());

        public void Add(LessonViewModel viewModel)
        {
            if (_repository.GetByCriteria(x => x.Schedule.Date == viewModel.Schedule.Date && x.Schedule.Hour == viewModel.Schedule.Hour) != null)
            {
                _bus.RaiseEvent(new Notification("Já existe uma aula cadastrada para este horario!"));
                return;
            }

            _repository.Add(_mapper.Map<Lesson>(viewModel));
            Commit();
        }

        public void Update(LessonViewModel viewModel)
        {
            var book = _repository.Get(viewModel.Id);

            if (book != null)
            {
                if (_repository.GetByCriteria(x => x.Schedule.Date == viewModel.Schedule.Date) != null)
                {
                    _bus.RaiseEvent(new Notification("Já existe uma aula cadastrada para este horario!"));
                    return;
                }

                _mapper.Map(viewModel, book);
                Commit();
            }
            else
            {
                _bus.RaiseEvent(new Notification("Título não encontrado"));
            }
        }

        public void Remove(LessonViewModel viewModel)
        {
            _repository.Remove(_repository.Get(viewModel.Id));
            Commit();
        }
    }
}
