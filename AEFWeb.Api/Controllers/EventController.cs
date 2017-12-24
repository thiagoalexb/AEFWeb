using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : BaseController
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _eventService = eventService;

        [HttpGet]
        [Route("get-all")]
        public IActionResult Get() => 
            Ok(_eventService.GetAll());

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var @event = _eventService.Get(id);
            if (@event == null) return NotFound();
            return Response(@event);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public IActionResult Post([FromBody]EventViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _eventService.Add(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public IActionResult Put([FromBody]EventViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _eventService.Update(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public IActionResult Delete([FromBody]EventViewModel entity)
        {
            _eventService.Remove(entity);

            return Response();
        }
    }
}
