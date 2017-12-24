using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Tag")]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService,
                                INotificationHandler<Notification> notifications) 
                                            : base(notifications) => _tagService = tagService;
        [HttpGet]
        [Route("get-all")]
        public IActionResult Get() => Ok(_tagService.GetAll());

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var tag = _tagService.Get(id);
            if (tag == null) return NotFound();
            return Response(tag);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public IActionResult Post([FromBody]TagViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _tagService.Add(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public IActionResult Put([FromBody]TagViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _tagService.Update(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public IActionResult Delete([FromBody]TagViewModel entity)
        {
            _tagService.Remove(entity);

            return Response();
        }
    }
}
