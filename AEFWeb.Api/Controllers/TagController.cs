using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AEFWeb.Api.Controllers.Base;
using MediatR;
using AEFWeb.Implementation.Notifications;
using AEFWeb.Core.ViewModels.Core;
using AEFWeb.Core.ViewModels;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Tag")]
    //[Authorize("Bearer")]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _tagService = tagService;

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get() => Ok(await _tagService.GetAllAsync());

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var book = await _tagService.GetAsync(id);
            if (book == null) return NotFound();
            return Response(book);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _tagService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpGet]
        [Route("auto-complete")]
        public async Task<IActionResult> GetAutoComplete(string search)
        {
            var autocomplete = await _tagService.GetAutoCompleteAsync(search);
            return Response(autocomplete);
        }
        
        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]TagViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _tagService.AddAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]TagViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _tagService.UpdateAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]TagViewModel entity)
        {
            await _tagService.RemoveAsync(entity);

            return Response();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]TagViewModel entity)
        {
            await _tagService.RestoreAsync(entity);

            return Response();
        }
    }
}
