using IndacoProject.Corso.Data.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Core
{
    public interface IEmailBackgroundService: IHostedService, IDisposable
    {
        Task EnqueMessage(MessageModel request, CancellationToken cancellationToken);
    }
}