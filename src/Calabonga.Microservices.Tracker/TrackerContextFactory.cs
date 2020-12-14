using Calabonga.Microservices.Tracker.Abstractions;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// Tracker Context Factory for TrackerContext accessing
    /// </summary>
    public class TrackerContextFactory : ITrackerContextFactory
    {
        private readonly ITrackerContextAccessor _trackerContextAccessor;

        /// <summary>
        /// Initializes a new instance of <see cref="TrackerContextFactory" />.
        /// </summary>
        public TrackerContextFactory() : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="TrackerContextFactory" />.
        /// </summary>
        public TrackerContextFactory(ITrackerContextAccessor trackerContextAccessor)
        {
            _trackerContextAccessor = trackerContextAccessor;
        }

        /// <summary>
        /// Creates a new <see cref="TrackerContext"/> with the tracker ID set for the current request.
        /// </summary>
        /// <param name="trackerId">The tracker ID to set on the context.</param>
        /// /// <param name="header">The header used to hold the tracker ID.</param>
        /// <returns>A new instance of a <see cref="TrackerContext"/>.</returns>
        public TrackerContext Create(string trackerId, string header)
        {
            var context = new TrackerContext(trackerId, header);

            if (_trackerContextAccessor != null)
            {
                _trackerContextAccessor.TrackerContext = context;
            }

            return context;
        }

        /// <summary>
        /// Disposes of the <see cref="TrackerContext"/> for the current request.
        /// </summary>
        public void Dispose()
        {
            if (_trackerContextAccessor != null)
            {
                _trackerContextAccessor.TrackerContext = null;
            }
        }
    }
}