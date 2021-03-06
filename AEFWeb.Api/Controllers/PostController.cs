﻿using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Get() => Ok(await _postService.GetAllAsync());

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var post = await _postService.GetAsync(id);
            if (post == null) return NotFound();
            return Response(post);
        }

        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _postService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]PostViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _postService.AddAsync(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]PostViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _postService.UpdateAsync(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]PostViewModel entity)
        {
            await _postService.RemoveAsync(entity);

            return Response();
        }

        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]PostViewModel entity)
        {
            await _postService.RestoreAsync(entity);

            return Response();
        }
    }
}
