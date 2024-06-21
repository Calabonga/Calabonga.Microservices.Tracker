using Microsoft.Extensions.Logging;

namespace Calabonga.Microservices.Tracker.Helpers
{
    /// <summary>
    /// Logger Event enumeration
    /// </summary>
    static class EventIdHelper
    {
        public static readonly EventId TrackerIdProcessingBegin = new EventId(1100, "TrackerIdProcessingBegin");

        public static readonly EventId TrackerIdProcessingEnd = new EventId(1101, "TrackerIdProcessingEnd");

        public static readonly EventId MissingTrackerIdProvider = new EventId(1102, "MissingTrackerIdProvider");

        public static readonly EventId EnforcedTrackerIdHeaderMissing = new EventId(1103, "EnforcedTrackerIdHeaderMissing");

        public static readonly EventId FoundTrackerIdHeader = new EventId(1104, "EnforcedTrackerIdHeaderMissing");

        public static readonly EventId MissingTrackerIdHeader = new EventId(1105, "MissingTrackerIdHeader");

        public static readonly EventId GeneratedHeaderUsingGeneratorFunction = new EventId(1106, "GeneratedHeaderUsingGeneratorFunction");

        public static readonly EventId GeneratedHeaderUsingProvider = new EventId(1107, "GeneratedHeaderUsingProvider");

        public static readonly EventId UpdatingTraceIdentifier = new EventId(1108, "UpdatingTraceIdentifier");

        public static readonly EventId CreatingTrackerContext = new EventId(1109, "CreatingTrackerContext");

        public static readonly EventId DisposingTrackerContext = new EventId(1110, "DisposingTrackerContext");

        public static readonly EventId WritingTrackerIdResponseHeader = new EventId(1111, "WritingTrackerIdResponseHeader");

        public static readonly EventId TrackerIdExcludesPathProcessing = new EventId(1112, "TrackerIdExcludesPathProcessing");

        public static readonly EventId TrackerIdExcludesHostProcessing = new EventId(1113, "TrackerIdExcludesHostProcessing");

        public static readonly EventId TrackerIdExcludesSchemeProcessing = new EventId(1114, "TrackerIdExcludesSchemeProcessing");
    }
}
