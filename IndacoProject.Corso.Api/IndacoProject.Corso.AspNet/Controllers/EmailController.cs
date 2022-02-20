using IndacoProject.Corso.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.AspNet.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("SendEmail")]
    public class EmailController : Controller
    {
        protected readonly IMediator _mediatr;

        public EmailController(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("sendemail")]
        public IActionResult Send()
        {
            return View();
        }

        [HttpPost("sendemail")]       
        public async Task<IActionResult> Send([FromForm]MessageModel model)
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

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
