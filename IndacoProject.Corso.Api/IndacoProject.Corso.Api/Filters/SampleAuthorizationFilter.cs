using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace IndacoProject.Corso.Api.Filters
{


    public class SampleAuthorizationFilterFactory : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) =>
            new SampleAuthorizationFilter(serviceProvider);

        private class SampleAuthorizationFilter : Attribute, IAuthorizationFilter
        {
            protected readonly IServiceProvider _serviceProvider;

            public SampleAuthorizationFilter(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var o = context.HttpContext.Request.Headers["Authorization"].ToString();
                var ts = _serviceProvider.GetRequiredService<ITokenService>();
                try
                {
                    var user = ts.VerifyRefreshToken(o);
                    context.HttpContext.User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user),
                                new Claim(ClaimTypes.Role, "")
                            }));
                    if (user == null)
                    {
                        context.Result = new ObjectResult(new { Message = "Invalid token", Code = 401 })
                        {
                            StatusCode = 401
                        };
                    }
                }
                catch (Exception e)
                {
                    context.Result = new ObjectResult(new { Message = e.Message, Code = 401 })
                    {
                        StatusCode = 401
                    };
                }
            }
        }
    }
}
