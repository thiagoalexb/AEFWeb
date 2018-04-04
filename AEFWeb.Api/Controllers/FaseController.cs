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
    [Route("api/Fase")]
    //[Authorize("Bearer")]
    public class FaseController : BaseController
    {
        private readonly IFaseService _faseService;

        public FaseController(IFaseService faseService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _faseService = faseService;

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get() => Ok(await _faseService.GetAllAsync());

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var fase = await _faseService.GetAsync(id);
            if (fase == null) return NotFound();
            return Response(fase);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _faseService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpGet]
        [Route("auto-complete")]
        public async Task<IActionResult> GetAutoComplete(string search)
        {
            var autocomplete = await _faseService.GetAutoCompleteAsync(search);
            return Response(autocomplete);
        }
        
        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]FaseViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _faseService.AddAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]FaseViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _faseService.UpdateAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]FaseViewModel entity)
        {
            await _faseService.RemoveAsync(entity);

            return Response();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]FaseViewModel entity)
        {
            await _faseService.RestoreAsync(entity);

            return Response();
        }
    }
}
