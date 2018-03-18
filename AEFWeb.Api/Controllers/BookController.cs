using AEFWeb.Api.Controllers.Base;
using AEFWeb.Api.Filters;
using AEFWeb.Core.Services;
using AEFWeb.Core.ViewModels;
using AEFWeb.Implementation.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AEFWeb.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Book")]
    //[Authorize("Bearer")]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService,
                                INotificationHandler<Notification> notifications) : base(notifications) =>
           _bookService = bookService;

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> Get() => Ok(await _bookService.GetAll());

        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var book = await _bookService.Get(id);
            if (book == null) return NotFound();
            return Response(book);
        }

        [HttpPost]
        [Route("add")]
        [TokenAddFilter]
        public async Task<IActionResult> Post([FromBody]BookViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _bookService.Add(entity);
            return Response(entity);
        }

        [HttpPut]
        [Route("update")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Put([FromBody]BookViewModel entity)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(entity);
            }
            await _bookService.Update(entity);
            return Response(entity);
        }

        [HttpDelete]
        [Route("delete")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Delete([FromBody]BookViewModel entity)
        {
            await _bookService.Remove(entity);

            return Response();
        }

        [HttpPatch]
        [Route("restore")]
        [TokenUpdateFilter]
        public async Task<IActionResult> Restore([FromBody]BookViewModel entity)
        {
            await _bookService.Restore(entity);

            return Response();
        }
    }
}
