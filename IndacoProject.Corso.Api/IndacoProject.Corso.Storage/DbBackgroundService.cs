using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Storage
{
    internal class DbBackgroundService: BackgroundService
    {
        protected readonly ILogger<DbBackgroundService> _logger;
        protected readonly IServiceProvider _serviceProvider;       

        public DbBackgroundService(ILogger<DbBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }
            await Task.CompletedTask;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
     
    }
}
