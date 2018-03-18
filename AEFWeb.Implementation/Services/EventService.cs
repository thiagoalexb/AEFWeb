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
using Newtonsoft.Json;

namespace AEFWeb.Implementation.Services
{
    public class EventService : Service<IEventRepository>//, IEventService
    {
        private readonly IMapper _mapper;
        private readonly ILessonRepository _lessonRepository;

        public EventService(IMapper mapper,
                            IMediatorHandler bus,
                            IUnitOfWork unitOfWork,
                            ILessonRepository lessonRepository) : base(bus, unitOfWork)
        {
            _mapper = mapper;
            _lessonRepository = lessonRepository;
        }

        //public EventViewModel Get(Guid id) => 
        //    _mapper.Map<EventViewModel>(_repository.Get(id));

        //public IEnumerable<EventViewModel> GetAll() => 
        //    _mapper.Map<IEnumerable<EventViewModel>>(_repository.GetAll());

        //public void Add(EventViewModel viewModel)
        //{
        //    //viewModel.Id = Guid.NewGuid();

        //    //if (_repository.GetByCriteria(x => x.Date.Date == viewModel.Date.Date) != null)
        //    //{
        //    //    _bus.RaiseEvent(new Notification("Já existe um evento cadastrado para esta data"));
        //    //    return;
        //    //}

        //    //bool existClass = false;
        //    //foreach (var schedule in viewModel.Lessons)
        //    //{
        //    //    if (_lessonRepository.GetByCriteria(x => x.Schedule.Hour == schedule.Schedule.Hour) != null)
        //    //    {
        //    //        _bus.RaiseEvent(new Notification("Já existe uma aula cadastrada para este horario!"));
        //    //        existClass = true;
        //    //        break;
        //    //    }
        //    //    schedule.Id = Guid.NewGuid();
        //    //}

        //    //var @event = _mapper.Map<Event>(viewModel);

        //    //if (!existClass)
        //    //{
        //    //    _repository.Add(@event);
        //    //    if(Commit())
        //    //        RegisterLog(new EventLog(Guid.NewGuid(), viewModel.CreationDate, viewModel.CreatorUserId, null, null, JsonConvert.SerializeObject(viewModel), Type, "Add"));
        //    //}
        //}

        //public void Update(EventViewModel viewModel)
        //{
        //    //var @event = _repository.Get(viewModel.Id);

        //    //if (@event != null)
        //    //{
        //    //    if (_repository.GetByCriteria(x => x.Id != viewModel.Id && x.Date.Date == viewModel.Date.Date) != null)
        //    //    {
        //    //        _bus.RaiseEvent(new Notification("Já existe um evento cadastrado para esta data"));
        //    //        return;
        //    //    }

        //    //    _lessonRepository.RemoveRange(@event.Lessons);
        //    //    Commit();

        //    //    bool existClass = false;
        //    //    foreach (var schedule in viewModel.Lessons)
        //    //    {
        //    //        if (_lessonRepository.GetByCriteria(x => x.Id != schedule.Id && x.Schedule.Hour == schedule.Schedule.Hour) != null)
        //    //        {
        //    //            _bus.RaiseEvent(new Notification("Já existe uma aula cadastrada para este horario!"));
        //    //            existClass = true;
        //    //            break;
        //    //        }
        //    //        schedule.EventId = @event.Id;
        //    //    }

        //    //    _mapper.Map(viewModel, @event);

        //    //    if (!existClass)
        //    //    {
        //    //        if (Commit())
        //    //            RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Update"));
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    _bus.RaiseEvent(new Notification("Título não encontrado"));
        //    //}
        //}

        //public void Remove(EventViewModel viewModel)
        //{
        //    var @event = _repository.Get(viewModel.Id);
        //    @event.SetDeleted(true);

        //    if (Commit())
        //        RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Remove"));
        //}

        //public void Restore(EventViewModel viewModel)
        //{
        //    var @event = _repository.Get(viewModel.Id);
        //    @event.SetDeleted(false);

        //    if (Commit())
        //        RegisterLog(new EventLog(Guid.NewGuid(), null, null, viewModel.LastUpdateDate, viewModel.LastUpdatedUserId, JsonConvert.SerializeObject(viewModel), Type, "Restore"));
        //}
    }
}
