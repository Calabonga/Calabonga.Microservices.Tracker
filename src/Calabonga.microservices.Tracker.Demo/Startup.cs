using Calabonga.microservices.Tracker.Demo.AppStart;
using Calabonga.microservices.Tracker.Demo.Controllers;
using Calabonga.microservices.Tracker.Demo.Infrastructure;
using Calabonga.Microservices.Tracker.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Calabonga.microservices.Tracker.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new TrackerOperationResultFilter());
            });
            
            services.AddHttpContextAccessor();

            services.AddTransient<IRequestProvider, RequestProvider>();
            services.AddTransient<IRequestFromConsumerService, RequestFromConsumerService>();
            services.AddTransient<IRequestService, RequestService>();

            ConfigureServicesMassTransit.ConfigureServices(services, Configuration);

            services.AddHttpClient("MyClient").AddCommunicationTrackerForwarding();

            // services.AddCommunicationTracker();
            // services.AddCommunicationTracker<CustomTrackerIdGenerator>();
            services.AddCommunicationTracker<CustomTrackerIdGenerator>(options =>
                {
                    // options.TrackerIdGenerator = () => "qweqweqwewqeqwe";
                    // options.EnforceHeader = false;
                    // options.IgnoreRequestHeader = false;
                    // options.RequestHeaderName = "X-Custom-Request-Trace-ID";
                    // options.ResponseHeaderName = "X-Custom-Response-Trace-ID";
                    // options.AddToLogger = true;
                    // options.LoggerScopeName = "MICROSERVICE_LOGGER";
                    // options.IncludeInResponse = true;
                    options.UpdateTraceIdentifier = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggingOptions = Configuration.GetSection("Log4NetCore").Get<Log4NetProviderOptions>();
            loggerFactory.AddLog4Net(loggingOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCommunicationTracker();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
