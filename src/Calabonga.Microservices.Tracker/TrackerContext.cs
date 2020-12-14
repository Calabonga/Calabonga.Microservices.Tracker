using System;
using Calabonga.Microservices.Tracker.Abstractions;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// Provides access to tracker properties.
    /// </summary>
    public class TrackerContext
    {
        /// <summary>
        /// The default tracker ID is used in cases where the tracker has not been set by the <see cref="ITrackerIdGenerator"/>.
        /// </summary>
        public const string DefaultTrackerId = "Not provided";

        /// <summary>
        /// Create a <see cref="TrackerContext"/> instance.
        /// </summary>
        /// <param name="trackerId">The tracker ID on the context.</param>
        /// <param name="header">The name of the header from which the Correlation ID was read/written.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="header"/> is null or empty.</exception>
        public TrackerContext(string trackerId, string header)
        {
            trackerId ??= DefaultTrackerId;

            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentException("A header must be provided.", nameof(header));
            }

            TrackerId = trackerId;
            Header = header;
        }

        /// <summary>
        /// The Correlation ID which is applicable to the current request.
        /// </summary>
        public string TrackerId { get; }

        /// <summary>
        /// The name of the header from which the Correlation ID was read/written.
        /// </summary>
        public string Header { get; }
    }
}
