using System.Threading;
using System.Threading.Tasks;
using Calabonga.microservices.Tracker.Demo.AppStart;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Calabonga.microservices.Tracker.Demo.Infrastructure
{
    public class RequestFromConsumerService: IRequestFromConsumerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RequestService> _logger;

        public RequestFromConsumerService(IHttpContextAccessor httpContextAccessor, ILogger<RequestService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public Task LogTraceId(IRequestItem item, CancellationToken cancellationToken)
        {

            var isNull = _httpContextAccessor.HttpContext == null ? "ПУСТО" : "ПОЛНО";
            _logger.LogInformation($"HttpContextAccessor: {isNull}");

            _logger.LogInformation($"{nameof(LogTraceId)}: {item.CorrelationId}");
            return Task.CompletedTask;
        }
    }

    public interface IRequestFromConsumerService
    {
        Task LogTraceId(IRequestItem item, CancellationToken cancellationToken);
    }
}
