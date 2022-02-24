using IndacoProject.Corso.Data.Options;
using IndacoProject.Corso.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IndacoProject.Corso.Storage.Extensions;
using IndacoProject.Corso.Services;
using IndacoProject.Corso.Storage;
using IndacoProject.Corso.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using IndacoProject.Corso.AspNet.Options;
using IndacoProject.Corso.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IndacoProject.Corso.AspNet
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
            services.AddSingleton<IEmailBackgroundService, EmailBackgroundService>();
            services.AddSingleton<ICipherService, CipherService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddHostedService(o => o.GetService<IEmailBackgroundService>());
            services.AddSingleton<BaseConfigService>();
            services.AddSingleton<IConfigureOptions<BaseConfig>, ConfigureBaseConfigOptions>();

            // TODO:  spostare in IdentityExtension
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            }).AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager();

            var _jwtConfig = Configuration.GetSection("Jwt").Get<JWTOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtConfig.Issuer,
                    ValidAudience = _jwtConfig.Audience,
                    LifetimeValidator = (before, expires, token, parameters) => expires > DateTime.UtcNow,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                    AuthenticationType = ClaimsIdentity.DefaultNameClaimType,
                    NameClaimType = ClaimTypes.Role,
                    RoleClaimType = ClaimTypes.Name
                };
            }).AddIdentityCookies(o => { });

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            }));

            services.ServiceAutoMapper();
            services.ServiceMediatr();
            services.AddMvc();
            services.AddControllersWithViews().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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
            });
        }
    }
}
