using AutoMapper;
using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using IndacoProject.Corso.Data.Options;
using IndacoProject.Corso.Storage;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace IndacoProject.Corso.Services
{
    public class EmailBackgroundService : BackgroundService, IEmailBackgroundService
    {
        protected readonly ILogger<EmailBackgroundService> _logger;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ConcurrentQueue<Message> _messages;
        protected readonly IMapper _mapper;
        private readonly AutoResetEvent _autoResetEvent;
        protected readonly SmtpOptions _smtpOptions;

        public EmailBackgroundService(ILogger<EmailBackgroundService> logger,
            IServiceProvider serviceProvider,
            IMapper mapper,
            IOptions<SmtpOptions> smtpOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _messages = new ConcurrentQueue<Message>();
            _mapper = mapper;
            _autoResetEvent = new AutoResetEvent(false);
            _smtpOptions = smtpOptions.Value;
        }

        public async Task EnqueMessage(MessageModel request, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<Message>(request);
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();      
                var messageRepo  = await messageRepository.Create(message, cancellationToken);
                _messages.Enqueue(messageRepo);
            }                     
            _autoResetEvent.Set();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await LoadMessages(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                Message message;
                while (true)
                {
                    stoppingToken.ThrowIfCancellationRequested();
                    if (_messages.TryDequeue(out message))
                    {
                        var res = false;
                        try
                        {
                            res = await this.Send(message, stoppingToken);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message, e);
                            res = false;
                        }
                        finally
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                                if (res)
                                {
                                    message.Sent = true;
                                }
                                else
                                {
                                    message.Attempts++;
                                }
                                await scopedProcessingService.Edit(message.Id, message, stoppingToken);
                            }
                        }
                    }
                    else
                    {
                        _ = Task.Delay(60000 * (_smtpOptions.CheckTimeout <= 0 ? 1 : _smtpOptions.CheckTimeout))
                                .ContinueWith(async (x) => await LoadMessages(stoppingToken));
                        _autoResetEvent.WaitOne();

                    }
                }
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async Task<List<Message>> GetMessages(CancellationToken token)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var messsageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                return await messsageRepository.ListUnsent(_smtpOptions.RetryAttempt, token);
            }
        }

        private async Task LoadMessages(CancellationToken token)
        {
            if (_messages.Count() == 0)
            {
                var messages = await GetMessages(token);
                foreach (var m in messages)
                {
                    if (!_messages.Any(o => o.Id == m.Id))
                    {
                        _messages.Enqueue(m);
                    }
                }
            }
            _autoResetEvent.Set();
        }

        private async Task<bool> Send(Message message, CancellationToken token)
        {
            MailboxAddress from = new MailboxAddress(message.Name, message.Email);
            MailboxAddress to = new MailboxAddress(_smtpOptions.FromName, _smtpOptions.FromAddress);
            var _message = new MimeMessage();
            var builder = new BodyBuilder() { TextBody = message.Body, HtmlBody= $"<p>{message.Body}</p>" };           
            
            var _xml = await MimeEntity.LoadAsync(new ContentType("text", "xml"), 
                    new MemoryStream(Encoding.UTF8.GetBytes("<note><to>Tove</to><from>Jani</from></note>")));   //message.XML
            _xml.ContentDisposition = new ContentDisposition();
            _xml.ContentDisposition.IsAttachment = true;
            _xml.ContentDisposition.FileName = "pippo.xml"; // message.FileName;

            builder.Attachments.Add(_xml);
            _message.Body = builder.ToMessageBody();
            _message.From.Add(to);
            _message.To.Add(to);
            _message.Subject = message.Subject;
            //_message.Body = new TextPart("plain")
            //{
            //    Text = message.Body
            //};
            _message.ReplyTo.Add(from);
            using (SmtpClient _smtpClient = new SmtpClient())
            {
                await _smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.SSL, token);
                await _smtpClient.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password, token);
                try
                {                    
                    await _smtpClient.SendAsync(_message);
                    return true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                    return false;
                }
            }
        }
    }
}
