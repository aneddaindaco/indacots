using IndacoProject.Corso.AspNet.Models;
using IndacoProject.Corso.AspNet.Models.BindingSample;
using IndacoProject.Corso.Data.Entities;
using IndacoProject.Corso.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IndacoProject.Corso.AspNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        protected readonly IMediator _mediatr;

        public HomeController(ILogger<HomeController> logger, IMediator mediatr)
        {
            _logger = logger;
            _mediatr = mediatr;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Messagio info");           

            return View();
        }

        public IActionResult Privacy([FromRoute]RouteBinding routeBinding, string id, [FromQuery]QueryBinding queryBinding)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
