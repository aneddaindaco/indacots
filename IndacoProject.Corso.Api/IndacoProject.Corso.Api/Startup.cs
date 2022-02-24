using IndacoProject.Corso.Data.Options;
using IndacoProject.Corso.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IndacoProject.Corso.Storage.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using IndacoProject.Corso.Api.Filters;
using System.IO;
using IndacoProject.Corso.Services.Extensions;
using IndacoProject.Corso.Api.Options;
using IndacoProject.Corso.Api.Hubs;

namespace IndacoProject.Corso.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<SmtpOptions>(Configuration.GetSection("Smtp"));
            services.Configure<JWTOptions>(Configuration.GetSection("Jwt"));
            services.AddSQLServer(Configuration);
            services.AddServices();
            services.AddSingleton<IConfigureOptions<BaseConfig>, ConfigureBaseConfigOptions>();
            services.AddAuthConfig(Configuration);
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:4200") // the Angular app url
                    .AllowCredentials();
            }));

            services.ServiceAutoMapper();
            services.ServiceMediatr();
            services.AddMvc();
            services.AddControllersWithViews(o =>
            {
                o.Filters.Add<SampleExceptionFilter>();
            }).AddNewtonsoftJson();
            services.AddSignalR();
            services.AddSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFileServer(env, "wwwroot");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSwagger();
                endpoints.MapGet("/prova/{name?}", async ctx =>
                {
                    var name = ctx.Request.RouteValues["name"] ?? "corso Indaco di Default";
                    await ctx.Response.WriteAsync($"Ciao {name}");
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotifyHub>("/notify");
            });
            app.IncludeSwagger();
        }
    }
}
