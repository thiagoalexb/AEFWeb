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
    [Route("api/Lesson")]
    //[Authorize("Bearer")]
    public class LessonController : BaseController
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _lessonService = lessonService;

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get() => Ok(await _lessonService.GetAllAsync());

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var book = await _lessonService.GetAsync(id);
            if (book == null) return NotFound();
            return Response(book);
        }

        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetPaginate(PaginateFilterBase filter)
        {
            var paginate = await _lessonService.GetPaginateAsync(filter);
            return Response(paginate);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]LessonViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            //await _lessonService.AddAsync(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]LessonViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
           // await _lessonService.UpdateAsync(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]LessonViewModel entity)
        {
            //await _lessonService.RemoveAsync(entity);

            return Response();
        }

        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]LessonViewModel entity)
        {
            //await _lessonService.RestoreAsync(entity);

            return Response();
        }
    }
}
