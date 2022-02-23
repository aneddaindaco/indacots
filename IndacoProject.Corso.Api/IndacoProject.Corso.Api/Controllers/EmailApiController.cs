using AutoMapper;
using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Models;
using IndacoProject.Corso.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Api.ControllersApi
{

    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/sendemail")]
    [ApiController]
    public class EmailApiController : ControllerBase
    {
        protected readonly IMediator _mediatr;
        protected readonly ILogger<EmailApiController> _logger;
        protected readonly IMessageRepository _messageRepository;
        protected readonly IMapper _mapper;

        public EmailApiController(IMediator mediatr,
            ILogger<EmailApiController> logger,
            IMessageRepository messageRepository,
            IMapper mapper)
        {
            _mediatr = mediatr;
            _logger = logger;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpGet("ExportCsv")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCsv(int page, int pagesize, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (pagesize == 0)
                    pagesize = 1;

                var messages = await _messageRepository.ListFailed(pagesize, page * pagesize, 3, cancellationToken);
                if (messages.Any())
                {
                    var csvMessages = messages.Select(x => _mapper.Map<MessageCsvModel>(x)).ToList();
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"{csvMessages.First().ToCsvHead()}\r\n");
                    csvMessages.ForEach(o => sb.Append($"{o.ToCsvLine()}\r\n"));

                    return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"messages-export_{DateTime.UtcNow:yyyyMMdd-HHmmss}.csv");
                }
                else
                {
                    return NotFound();
                }

            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(o => o.ErrorMessage);
            return BadRequest(new { errors = errors, stato = "Failed" });

        }

        [HttpGet("ListUnsend")]
        [SampleResultFilter]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MessageModel>>> Get(int page, int pagesize, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (pagesize == 0)
                    pagesize = 1;

                var messages = await _messageRepository.ListFailed(pagesize, page * pagesize, 3, cancellationToken);
                return Ok(messages.Select(x => _mapper.Map<MessageModel>(x)));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(o => o.ErrorMessage);
            return BadRequest(new { errors = errors, stato = "Failed" });

        }

        [HttpPost("sendemail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Send([FromBody] MessageModel model)
        {
            if (ModelState.IsValid)
            {
                //model.Body = "Corpo messaggio bus";
                //model.Email = "mario.monti@gov.it";
                //model.Subject = "Invio richiesta chiarimenti";
                //model.Name = "Mario Monti";

                model.Name ??= "Username";
                model.Subject ??= "Subject";

                await _mediatr.Send(model);

                return Ok();

            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(o => o.ErrorMessage);
            return BadRequest(new { errors = errors, stato = "Failed" });

        }
    }
}
