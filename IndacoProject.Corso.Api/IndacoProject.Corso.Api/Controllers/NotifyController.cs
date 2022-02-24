using AutoMapper;
using IndacoProject.Corso.Core;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using IndacoProject.Corso.Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IndacoProject.Corso.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IndacoProject.Corso.Api.ControllersApi
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/notify")]
    [ApiController]
    public class NotifyController : Controller
    {
        private readonly IHubContext<NotifyHub, INotify> _hubContext;

        public NotifyController(IHubContext<NotifyHub, INotify> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("send-message")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Error([FromBody] SendNotifyMessage model)
        {
            _hubContext.Clients.All.DisplayMessage(model.Message);
            return Ok();
        }
    }
}
