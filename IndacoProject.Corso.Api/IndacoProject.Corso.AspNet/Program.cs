using IndacoProject.Corso.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.AspNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateBootstrapLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureServices((context, services) =>
                 {
                     var c = UseCustomConfig(context.Configuration, context.HostingEnvironment);
                     context.Configuration = c;
                 })
                .UseSerilog((context, services, config) =>
                    config.ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static IConfiguration UseCustomConfig(IConfiguration _configuration, IHostEnvironment _env)
        {             
            var b = ((ConfigurationRoot)_configuration).Providers.FirstOrDefault(o => o.GetType() == typeof(CommandLineConfigurationProvider));
            string file;
            if (!b.TryGet("config", out file))
            {
                if (!File.Exists("Config/default.json"))
                {
                    Environment.Exit(-1);
                }
                var builder = new ConfigurationBuilder()
                    .AddConfiguration(_configuration)
                    .SetBasePath(_env.ContentRootPath)
                    .AddJsonFile("Config/default.json", optional: false, reloadOnChange: true);
                Console.Title = $"Indaco Project - Config: default";
                return builder.Build();
            }
            else
            {
                if (!File.Exists($"Config/{file}.json"))
                {
                    Environment.Exit(-1);
                }
                var builder = new ConfigurationBuilder()
                    .AddConfiguration(_configuration)
                    .SetBasePath(_env.ContentRootPath)
                    .AddJsonFile($"Config/{file}.json", optional: false, reloadOnChange: true);
                Console.Title = $"Indaco Project - Config: {file}";
                return builder.Build();
            }
        }
    }
}
