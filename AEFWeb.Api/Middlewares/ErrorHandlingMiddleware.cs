using AEFWeb.Core.Services;
using AEFWeb.Data.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AEFWeb.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private IErrorLogService _errorLogService;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IErrorLogService errorLogService)
        {
            try
            {
                _errorLogService = errorLogService;
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _errorLogService.Add(new ErrorLog(Guid.NewGuid(), GetErrorMessage(exception), exception.ToString()));

            var result = JsonConvert.SerializeObject(new { error = "Some error(s) occoured." });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }

        private static string GetErrorMessage(Exception exception)
        {
            string message = exception?.InnerException?.Message;

            if (string.IsNullOrWhiteSpace(message))
                message = exception?.Message;

            return message;
        }
    }
}
