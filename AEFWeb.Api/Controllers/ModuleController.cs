﻿using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Core.ViewModels.Core;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Module")]
    //[Authorize("Bearer")]
    public class ModuleController : BaseController
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _moduleService = moduleService;

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get() => Ok(await _moduleService.GetAllAsync());

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var module = await _moduleService.GetAsync(id);
            if (module == null) return NotFound();
            return Response(module);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _moduleService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpGet]
        [Route("auto-complete")]
        public async Task<IActionResult> GetAutoComplete(string search)
        {
            var autocomplete = await _moduleService.GetAutoCompleteAsync(search);
            return Response(autocomplete);
        }
        
        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]ModuleViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _moduleService.AddAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]ModuleViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _moduleService.UpdateAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]ModuleViewModel entity)
        {
            await _moduleService.RemoveAsync(entity);

            return Response();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]ModuleViewModel entity)
        {
            await _moduleService.RestoreAsync(entity);

            return Response();
        }
    }
}
