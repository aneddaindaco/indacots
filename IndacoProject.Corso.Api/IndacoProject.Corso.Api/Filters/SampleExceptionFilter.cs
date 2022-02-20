using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;

namespace IndacoProject.Corso.Api.Filters
{
    public class SampleExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public SampleExceptionFilter(IHostEnvironment hostEnvironment) => _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
            if (!_hostEnvironment.IsDevelopment())
            {
                context.Result = new ObjectResult(new
                {
                    Code = 500,
                    Message = context.Exception.Message
                })
                {
                    StatusCode = 500,
                    ContentTypes = new MediaTypeCollection() { new MediaTypeHeaderValue("application/json") }
                };
                return;
            }

            context.Result = new ObjectResult(new
            {
                StackTrace = context.Exception.StackTrace,
                Code = 500,
                Message = context.Exception.Message
            })
            {
                StatusCode = 500,
                ContentTypes = new MediaTypeCollection() { new MediaTypeHeaderValue("application/json") }
            };
        }
    }
}
