using System;
using System.Linq;
using Calabonga.Microservices.Tracker.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Calabonga.Microservices.Tracker.Extensions
{
    /// <summary>
    /// See <see cref="IServiceCollection"/> to register Tracker
    /// </summary>
    public static class TrackerServiceCollectionExtensions
    {
        private const string MultipleProviderExceptionMessage = "A provider has already been registered. Only a single provider may be registered.";

        /// <summary>
        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// /// <remarks>
        /// This operation is idempotent - multiple invocations will still only result in a single
        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
        /// <returns></returns>
        public static ITrackerBuilder AddCommunicationTracker(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<ITrackerContextAccessor, TrackerContextAccessor>();
            services.TryAddTransient<ITrackerContextFactory, TrackerContextFactory>();

            if (services.Any(x => x.ServiceType == typeof(ITrackerIdGenerator)))
            {
                throw new InvalidOperationException(MultipleProviderExceptionMessage);
            }

            services.TryAddSingleton<ITrackerIdGenerator, DefaultTrackerIdGenerator>();

            return new TrackerBuilder(services);
        }

        /// <summary>
        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// /// <remarks>
        /// This operation is idempotent - multiple invocations will still only result in a single
        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
        /// <returns></returns>
        public static ITrackerBuilder AddCommunicationTracker<T>(this IServiceCollection services) where T : class, ITrackerIdGenerator
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<ITrackerContextAccessor, TrackerContextAccessor>();
            services.TryAddTransient<ITrackerContextFactory, TrackerContextFactory>();

            if (services.Any(x => x.ServiceType == typeof(ITrackerIdGenerator)))
            {
                throw new InvalidOperationException(MultipleProviderExceptionMessage);
            }

            services.TryAddSingleton<ITrackerIdGenerator, T>();

            return new TrackerBuilder(services);
        }

        /// <summary>
        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>
        /// This operation is idempotent - multiple invocations will still only result in a single
        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
        /// <typeparam name="T">The <see cref="ITrackerIdGenerator"/> implementation type.</typeparam>
        /// <param name="trackerOptions">Configure action for options</param>
        /// <returns>An instance of <see cref="ITrackerBuilder"/> which to be used to trackerOptions correlation ID providers and options.</returns>
        public static ITrackerBuilder AddCommunicationTracker<T>(this IServiceCollection services, Action<TrackerOptions> trackerOptions) where T : class, ITrackerIdGenerator
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (trackerOptions is null)
            {
                throw new ArgumentNullException(nameof(trackerOptions));
            }

            services.TryAddSingleton<ITrackerContextAccessor, TrackerContextAccessor>();
            services.TryAddTransient<ITrackerContextFactory, TrackerContextFactory>();

            if (services.Any(x => x.ServiceType == typeof(ITrackerIdGenerator)))
            {
                throw new InvalidOperationException(MultipleProviderExceptionMessage);
            }

            services.TryAddSingleton<ITrackerIdGenerator, T>();

            services.Configure(trackerOptions);


            return new TrackerBuilder(services);
        }

        /// <summary>
        /// Adds required services to support the Tracker ID functionality to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <remarks>
        /// This operation is idempotent - multiple invocations will still only result in a single
        /// instance of the required services in the <see cref="IServiceCollection"/>. It can be invoked
        /// multiple times in order to get access to the <see cref="ITrackerBuilder"/> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the correlation ID services to.</param>
        /// <typeparam name="T">The <see cref="ITrackerIdGenerator"/> implementation type.</typeparam>
        /// <param name="trackerOptions">Configure action for options</param>
        /// <param name="excludeOptions"></param>
        /// <returns>An instance of <see cref="ITrackerBuilder"/> which to be used to trackerOptions correlation ID providers and options.</returns>
        public static ITrackerBuilder AddCommunicationTracker<T>(this IServiceCollection services, Action<TrackerOptions> trackerOptions, Action<ExcludeOptions> excludeOptions) where T : class, ITrackerIdGenerator
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (trackerOptions is null)
            {
                throw new ArgumentNullException(nameof(trackerOptions));
            }

            services.TryAddSingleton<ITrackerContextAccessor, TrackerContextAccessor>();
            services.TryAddTransient<ITrackerContextFactory, TrackerContextFactory>();

            if (services.Any(x => x.ServiceType == typeof(ITrackerIdGenerator)))
            {
                throw new InvalidOperationException(MultipleProviderExceptionMessage);
            }

            services.TryAddSingleton<ITrackerIdGenerator, T>();
            services.Configure(trackerOptions);
            services.Configure(excludeOptions);

            return new TrackerBuilder(services);
        }
    }
}
