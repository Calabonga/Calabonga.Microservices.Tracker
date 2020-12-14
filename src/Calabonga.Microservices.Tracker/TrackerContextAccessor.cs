using System.Threading;
using Calabonga.Microservices.Tracker.Abstractions;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// TrackerContext accessor
    /// </summary>
    public class TrackerContextAccessor : ITrackerContextAccessor
    {
        private static readonly AsyncLocal<TrackerContext> TrackerContextLocal = new AsyncLocal<TrackerContext>();

        public TrackerContext TrackerContext
        {
            get => TrackerContextLocal.Value;
            set => TrackerContextLocal.Value = value;
        }
    }
}