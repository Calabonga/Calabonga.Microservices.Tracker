using Calabonga.Microservices.Tracker.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Microservices.Tracker
{
    /// <inheritdoc />
    internal class TrackerBuilder : ITrackerBuilder
    {
        public TrackerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}