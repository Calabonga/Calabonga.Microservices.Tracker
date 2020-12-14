using System;
using Microsoft.Extensions.Logging;

namespace Calabonga.Microservices.Tracker.Helpers
{
    static class LogHelper
    {
        private static readonly Action<ILogger, Exception> TrackerIdProcessingBeginExecute =
            LoggerMessage.Define(
                LogLevel.Debug,
                EventIdHelper.TrackerIdProcessingBegin,
                "Running tracker ID processing");

        private static readonly Action<ILogger, string, Exception> TrackerIdProcessingEndExecute =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                EventIdHelper.TrackerIdProcessingEnd,
                "Tracker ID processing was completed with a final tracker ID {TrackerId}");

        private static readonly Action<ILogger, Exception> MissingTrackerIdProviderExecute =
            LoggerMessage.Define(
                LogLevel.Error,
                EventIdHelper.MissingTrackerIdProvider,
                "Tracker ID middleware was called when no ITrackerIdProvider had been configured");

        private static readonly Action<ILogger, Exception> EnforcedTrackerIdHeaderMissingExecute =
            LoggerMessage.Define(
                LogLevel.Warning,
                EventIdHelper.EnforcedTrackerIdHeaderMissing,
                "Tracker ID header is enforced but no Tracker ID was not found in the request headers");

        private static readonly Action<ILogger, string, Exception> FoundTrackerIdHeaderExecute =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIdHelper.FoundTrackerIdHeader,
                "Tracker ID {TrackerId} was found in the request headers");

        private static readonly Action<ILogger, Exception> MissingTrackerIdHeaderExecute = 
            LoggerMessage.Define(
                LogLevel.Information,
                EventIdHelper.MissingTrackerIdHeader,
                "No tracker ID was found in the request headers");

        private static readonly Action<ILogger, string, Exception> GeneratedHeaderUsingGeneratorFunctionExecute =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                EventIdHelper.GeneratedHeaderUsingGeneratorFunction,
                "Generated a tracker ID {TrackerId} using the configured generator function");

        private static readonly Action<ILogger, string, Type, Exception> GeneratedHeaderUsingProviderExecute =
            LoggerMessage.Define<string, Type>(
                LogLevel.Debug,
                EventIdHelper.GeneratedHeaderUsingProvider,
                "Generated a tracker ID {TrackerId} using the {Type} provider");

        private static readonly Action<ILogger, Exception> UpdatingTraceIdentifierExecute = 
            LoggerMessage.Define(
                LogLevel.Debug,
                EventIdHelper.UpdatingTraceIdentifier,
                "Updating the TraceIdentifier value on the HttpContext");

        private static readonly Action<ILogger, Exception> CreatingTrackerContextExecute = 
            LoggerMessage.Define(
                LogLevel.Debug,
                EventIdHelper.CreatingTrackerContext,
                "Creating the tracker context for this request");

        private static readonly Action<ILogger, Exception> DisposingTrackerContextExecute = 
            LoggerMessage.Define(
                LogLevel.Debug,
                EventIdHelper.DisposingTrackerContext,
                "Disposing the tracker context for this request");

        private static readonly Action<ILogger, string, string, Exception> WritingTrackerIdResponseHeaderExecute =
            LoggerMessage.Define<string, string>(
                LogLevel.Debug,
                EventIdHelper.WritingTrackerIdResponseHeader,
                "Writing tracker ID response header {ResponseHeader} with value {TrackerId}");

        public static void TrackerIdProcessingBegin(ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                TrackerIdProcessingBeginExecute(logger, null);
            }
        }

        public static void TrackerIdProcessingEnd(ILogger logger, string trackerId)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                TrackerIdProcessingEndExecute(logger, trackerId, null);
            }
        }

        public static void MissingTrackerIdProvider(ILogger logger) => 
            MissingTrackerIdProviderExecute(logger, null);

        public static void EnforcedTrackerIdHeaderMissing(ILogger logger) => 
            EnforcedTrackerIdHeaderMissingExecute(logger, null);

        public static void FoundTrackerIdHeader(ILogger logger, string trackerId) =>
            FoundTrackerIdHeaderExecute(logger, trackerId, null);

        public static void MissingTrackerIdHeader(ILogger logger) => 
            MissingTrackerIdHeaderExecute(logger, null);

        public static void GeneratedHeaderUsingGeneratorFunction(ILogger logger, string trackerId) => 
            GeneratedHeaderUsingGeneratorFunctionExecute(logger, trackerId, null);

        public static void GeneratedHeaderUsingProvider(ILogger logger, string trackerId, Type type) => 
            GeneratedHeaderUsingProviderExecute(logger, trackerId, type, null);

        public static void UpdatingTraceIdentifier(ILogger logger) => 
            UpdatingTraceIdentifierExecute(logger, null);

        public static void CreatingTrackerContext(ILogger logger) => 
            CreatingTrackerContextExecute(logger, null);

        public static void DisposingTrackerContext(ILogger logger) => 
            DisposingTrackerContextExecute(logger, null);

        public static void WritingTrackerIdResponseHeader(ILogger logger, string headerName, string trackerId) =>
            WritingTrackerIdResponseHeaderExecute(logger, headerName, trackerId, null);
    }
}