using System.Net.Http;
using System.Threading.Tasks;
using Calabonga.OperationResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Calabonga.microservices.Tracker.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ILogger<ActivitiesController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public ActivitiesController(
            ILogger<ActivitiesController> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<ActionResult<OperationResult<ResponseViewModel>>> Get()
        {
            var operation = OperationResult.CreateResult<ResponseViewModel>();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.calabonga.com/api3/v3/random");
            var client = _clientFactory.CreateClient("MyClient");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                operation.Result = new ResponseViewModel(responseStream);
            }
            else
            {
                _logger.LogInformation(response.Version.ToString());
            }

            _logger.LogInformation($"HttpContext.TraceIdentifier: {HttpContext.TraceIdentifier}");
            _logger.LogInformation(JsonConvert.SerializeObject(operation));
            return Ok(operation);
        }
    }

    public class TrackerOperationResultFilter : IActionFilter
    {
        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is OkObjectResult result)
            {
                //((OperationResult)result.Value).ActivityId = context.HttpContext.TraceIdentifier;
            }
        }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }

    public class ResponseViewModel
    {
        public ResponseViewModel(string content)
        {
            Content = content;

        }

        public string Content { get; }
    }
}