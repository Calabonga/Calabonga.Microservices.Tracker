using System;
using System.Threading.Tasks;
using Calabonga.microservices.Tracker.Demo.Infrastructure;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.microservices.Tracker.Demo.AppStart
{
    /// <summary>
    /// MassTransit configurations for ASP.NET Core
    /// </summary>
    public class ConfigureServicesMassTransit
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var massTransitSection = configuration.GetSection("MassTransit");
            var url = massTransitSection.GetValue<string>("Url");
            var host = massTransitSection.GetValue<string>("Host");
            var userName = massTransitSection.GetValue<string>("UserName");
            var password = massTransitSection.GetValue<string>("Password");
            if (massTransitSection == null || url == null || host == null)
            {
                throw new ArgumentNullException("Section 'mass-transit' configuration settings are not found in appSettings.json");
            }

            services.AddMassTransit(x =>
            {
                x.AddBus(busFactory =>
                {
                    var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host($"rabbitmq://{url}/{host}", configurator =>
                        {
                            configurator.Username(userName);
                            configurator.Password(password);
                        });

                        cfg.ConfigureEndpoints(busFactory, KebabCaseEndpointNameFormatter.Instance);

                        cfg.UseJsonSerializer();

                        cfg.UseHealthCheck(busFactory);
                    });

                    return bus;
                });
                x.AddRequestClient<IRequestItem>();
                x.AddConsumer<RequestConsumer>();
            });

            services.AddMassTransitHostedService();
        }
    }

    public class RequestConsumer : IConsumer<IRequestItem>
    {
        private readonly IRequestFromConsumerService _requestFromConsumerService;
        private readonly ILogger<RequestConsumer> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestConsumer(
            IRequestFromConsumerService requestFromConsumerService,
            ILogger<RequestConsumer> logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _requestFromConsumerService = requestFromConsumerService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task Consume(ConsumeContext<IRequestItem> context)
        {
            var requestItem = context.Message;

            var id = requestItem.CorrelationId;
            _logger.LogInformation($"Trace ID: {id}");
            
            var isNull = _httpContextAccessor.HttpContext == null;
            _logger.LogInformation($"[Consumer] Starting...HttpContext == null ? {isNull}");

            _requestFromConsumerService.LogTraceId(requestItem, context.CancellationToken);

            return context.RespondAsync(requestItem);
        }
    }

    public class RequestItem : IRequestItem
    {
        public Guid CorrelationId { get; set; }
    }

    public interface IRequestItem : CorrelatedBy<Guid>
    {
    }
}
