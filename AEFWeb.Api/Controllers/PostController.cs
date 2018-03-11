﻿using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Post")]
    //[Authorize("Bearer")]
    public class PostController : BaseController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _postService = postService;

        [HttpGet]
        [Route("get-all")]
        public IActionResult Get() => Ok(_postService.GetAll());

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var post = _postService.Get(id);
            if (post == null) return NotFound();
            return Response(post);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public IActionResult Post([FromBody]PostViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _postService.Add(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public IActionResult Put([FromBody]PostViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            _postService.Update(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public IActionResult Delete([FromBody]PostViewModel entity)
        {
            _postService.Remove(entity);

            return Response();
        }

        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public IActionResult Restore([FromBody]PostViewModel entity)
        {
            _postService.Restore(entity);

            return Response();
        }
    }
}
