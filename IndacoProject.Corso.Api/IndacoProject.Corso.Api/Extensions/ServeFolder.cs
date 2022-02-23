using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Reflection;

namespace IndacoProject.Corso.Api.Extensions
{
    public static class MediatrExtensionser
    {
        public static IApplicationBuilder UseFileServer(this IApplicationBuilder app, IWebHostEnvironment env,  string folder)
        {
            var _configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), folder);
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, _configPath)),
                RequestPath = ""
            });
            return app;
        }
    }
}
