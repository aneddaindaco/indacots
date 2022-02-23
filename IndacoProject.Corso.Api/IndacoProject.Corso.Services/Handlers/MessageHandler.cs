using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Services.Handlers
{
    public class MessageHandler : AsyncRequestHandler<MessageModel>
    {
        protected readonly ILogger<MessageHandler> _logger;
        protected readonly IEmailBackgroundService _emailBackgroundService;

        public MessageHandler(ILogger<MessageHandler> logger, IEmailBackgroundService emailBackgroundService)
        {
            _logger = logger;
            _emailBackgroundService = emailBackgroundService;
        }        

        protected async override Task Handle(MessageModel request, CancellationToken cancellationToken)
        {
            await _emailBackgroundService.EnqueMessage(request, cancellationToken);
            _logger.LogInformation("Message Enque");           
        }
    }
}
