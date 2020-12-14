using System.Threading;
using System.Threading.Tasks;

using Calabonga.microservices.Tracker.Demo.AppStart;

using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Calabonga.microservices.Tracker.Demo.Infrastructure
{
    public class RequestService : IRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestClient<IRequestItem> _requestClient;
        private readonly ILogger<RequestService> _logger;
        public RequestService(
            IHttpContextAccessor httpContextAccessor,
            IRequestClient<IRequestItem> requestClient, 
            ILogger<RequestService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task<IRequestItem> Send(IRequestItem item, CancellationToken cancellationToken)
        {
            var isNull = _httpContextAccessor.HttpContext == null;
            _logger.LogInformation($"[Provider] Starting...HttpContext == null ? {isNull}");
            var response = await _requestClient.GetResponse<IRequestItem>(item, cancellationToken, RequestTimeout.Default);
            _logger.LogInformation("Ending...");
            return response.Message;
        }
    }

    public interface IRequestService
    {
        Task<IRequestItem> Send(IRequestItem item, CancellationToken cancellationToken);
    }
}
