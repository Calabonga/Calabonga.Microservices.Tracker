using System;
using Calabonga.Microservices.Tracker.Abstractions;
using Microsoft.AspNetCore.Builder;

namespace Calabonga.Microservices.Tracker.Extensions
{
    /// <summary>
    /// Extension methods for the CorrelationIdMiddleware.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables correlation IDs for the request.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseCommunicationTracker(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (app.ApplicationServices.GetService(typeof(ITrackerContextFactory)) == null)
            {
                throw new InvalidOperationException(
                    "Unable to find the required services. You must call the appropriate AddCorrelationId or AddDefaultCorrelationId method in ConfigureServices in the application startup code.");
            }

            return app.UseMiddleware<TrackerMiddleware>();
        }
    }
}
