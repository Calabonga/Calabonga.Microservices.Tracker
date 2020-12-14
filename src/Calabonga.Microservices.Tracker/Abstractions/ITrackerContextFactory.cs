namespace Calabonga.Microservices.Tracker.Abstractions
{
    /// <summary>
    /// A factory for creating and disposing an instance of a <see cref="TrackerContext"/>.
    /// </summary>
    public interface ITrackerContextFactory
    {
        /// <summary>
        /// Creates a new <see cref="TrackerContext"/> with the tracker ID set for the current request.
        /// </summary>
        /// <param name="trackerId">The tracker ID to set on the context.</param>
        /// /// <param name="header">The header used to hold the tracker ID.</param>
        /// <returns>A new instance of a <see cref="TrackerContext"/>.</returns>
        TrackerContext Create(string trackerId, string header);

        /// <summary>
        /// Disposes of the <see cref="TrackerContext"/> for the current request.
        /// </summary>
        void Dispose();
    }
}
