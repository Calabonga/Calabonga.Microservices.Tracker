using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Microservices.Tracker.Abstractions
{
    /// <summary>
    /// A builder used to configure the tracker ID services.
    /// </summary>
    public interface ITrackerBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> into which the tracker ID services will be registered.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
