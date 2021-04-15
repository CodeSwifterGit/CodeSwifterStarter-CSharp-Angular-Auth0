using System;
using System.Net;
using CodeSwifterStarter.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CodeSwifterStarter.Web.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context != null)
            {
                if (context.Exception is ValidationException exception)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(
                        exception.Failures);

                    return;
                }

                if (context.Exception is DbUpdateException updateException)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(
                        updateException.InnerException?.Message ?? "Unknown error");

                    return;
                }

                var code = HttpStatusCode.InternalServerError;

                if (context.Exception is NotFoundException) code = HttpStatusCode.NotFound;

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int) code;
                context.Result = new JsonResult(new
                {
                    error = new[] {context.Exception.Message},
                    stackTrace = context.Exception.StackTrace
                });
            }
        }
    }
}