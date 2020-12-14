namespace Calabonga.Microservices.Tracker.Abstractions
{
    /// <summary>
    /// Provides access to the <see cref="TrackerContext"/> for the current request.
    /// </summary>
    public interface ITrackerContextAccessor
    {
        TrackerContext TrackerContext { get; set; }
    }
}
