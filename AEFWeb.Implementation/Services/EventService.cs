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
    public class EventService : Service<IEventRepository>, IEventService
    {
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;

        public EventService(IMapper mapper,
                            IMediatorHandler bus,
                            IUnitOfWork unitOfWork,
                            ILessonService lessonService) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _lessonService = lessonService;
        }

        public EventViewModel Get(Guid id) => 
            _mapper.Map<EventViewModel>(_repository.Get(id));

        public IEnumerable<EventViewModel> GetAll() => 
            _mapper.Map<IEnumerable<EventViewModel>>(_repository.GetAll());

        public void Add(EventViewModel viewModel)
        {
            if (_repository.GetByCriteria(x => x.Date.Date == viewModel.Date.Date) != null)
            {
                _bus.RaiseEvent(new Notification("Já existe um evento cadastrado para esta data"));
                return;
            }
            var @event = _mapper.Map<Event>(viewModel);
            _repository.Add(@event);
            if (Commit())
            {
                foreach (var item in viewModel.Lessons)
                {
                    item.EventId = @event.Id;
                    _lessonService.Add(item);
                }
            }
        }

        public void Update(EventViewModel viewModel)
        {
            var @event = _repository.Get(viewModel.Id);

            if (@event != null)
            {
                if (_repository.GetByCriteria(x => x.Date.Date == viewModel.Date.Date) != null)
                {
                    _bus.RaiseEvent(new Notification("Já existe um evento cadastrado para esta data"));
                    return;
                }

                _mapper.Map(viewModel, @event);
                Commit();
            }
            else
            {
                _bus.RaiseEvent(new Notification("Título não encontrado"));
            }
        }

        public void Remove(EventViewModel viewModel)
        {
            _repository.Remove(_repository.Get(viewModel.Id));
            Commit();
        }
    }
}
