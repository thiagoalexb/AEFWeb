using AEFWeb.Api.Controllers.Base;
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
    [Route("api/SpecialWeek")]
    //[Authorize("Bearer")]
    public class SpecialWeekController : BaseController
    {
        private readonly ISpecialWeekService _specialWeekService;

        public SpecialWeekController(ISpecialWeekService specialWeekService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _specialWeekService = specialWeekService;

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get() => Ok(await _specialWeekService.GetAllAsync());

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var specialWeek = await _specialWeekService.GetAsync(id);
            if (specialWeek == null) return NotFound();
            return Response(specialWeek);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _specialWeekService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpGet]
        [Route("auto-complete")]
        public async Task<IActionResult> GetAutoComplete(string search)
        {
            var autocomplete = await _specialWeekService.GetAutoCompleteAsync(search);
            return Response(autocomplete);
        }
        
        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]SpecialWeekViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }

            await _specialWeekService.AddAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]SpecialWeekViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _specialWeekService.UpdateAsync(entity);
            return Response(entity);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]SpecialWeekViewModel entity)
        {
            await _specialWeekService.RemoveAsync(entity);

            return Response();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]SpecialWeekViewModel entity)
        {
            await _specialWeekService.RestoreAsync(entity);

            return Response();
        }
    }
}
