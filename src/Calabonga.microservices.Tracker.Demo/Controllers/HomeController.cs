using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Calabonga.microservices.Tracker.Demo.AppStart;
using Calabonga.microservices.Tracker.Demo.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calabonga.microservices.Tracker.Demo.Models;
using Microsoft.AspNetCore.Http;

namespace Calabonga.microservices.Tracker.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRequestProvider _requestProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IRequestProvider requestProvider,
            IHttpContextAccessor httpContextAccessor,
            ILogger<HomeController> logger)
        {
            _requestProvider = requestProvider;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Privacy()
        {
            var key = _httpContextAccessor?.HttpContext?.TraceIdentifier;
            _logger.LogInformation($"TraceIdentifier: {key}");

            var traceId = Guid.Parse("ac5d58da-4aea-78b8-4f6a-432c8454f511");

            var item = new RequestItem
            {
                CorrelationId = traceId
            };

            var response = await _requestProvider.SendRequest(item, HttpContext.RequestAborted);
            ViewBag.Message = response.CorrelationId;
            return View();
        }

        public IActionResult Index()
        {
            var isEmpty = _httpContextAccessor.HttpContext == null;
            _logger.LogInformation(isEmpty.ToString());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
