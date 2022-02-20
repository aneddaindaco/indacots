using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace IndacoProject.Corso.Api.Filters
{
    public class SampleResultFilter : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult data && data.ContentTypes.Contains("application/json"))
            {
                data.Value = new { Data = data.Value, Message = "Success", Code = data.StatusCode };
            }
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}                                            
