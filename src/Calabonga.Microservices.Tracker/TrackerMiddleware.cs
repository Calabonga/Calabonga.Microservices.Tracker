using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calabonga.Microservices.Tracker.Abstractions;
using Calabonga.Microservices.Tracker.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// Tracker Middleware
    /// </summary>
    public class TrackerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ITrackerIdGenerator _trackerIdGenerator;
        private readonly TrackerOptions _options;

        /// <summary>
        /// Creates a new instance of the TrackerMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The <see cref="ILogger"/> instance to log to.</param>
        /// <param name="options">The configuration options.</param>
        /// <param name="trackerIdGenerator"></param>
        public TrackerMiddleware(
            RequestDelegate next,
            ILogger<TrackerMiddleware> logger,
            IOptions<TrackerOptions> options,
            ITrackerIdGenerator trackerIdGenerator = null)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _trackerIdGenerator = trackerIdGenerator;
            _options = options.Value;
        }

        /// <summary>
        /// Processes a request to synchronise TraceIdentifier and Tracker ID headers. Also creates a 
        /// <see cref="TrackerContext"/> for the current request and disposes of it when the request is completing.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="trackerContextFactory">The <see cref="ITrackerContextFactory"/> which can create a <see cref="TrackerContext"/>.</param>
        public async Task Invoke(HttpContext context, ITrackerContextFactory trackerContextFactory)
        {
            LogHelper.TrackerIdProcessingBegin(_logger);

            if (_trackerIdGenerator is null)
            {
                LogHelper.MissingTrackerIdProvider(_logger);

                throw new InvalidOperationException("No 'ITrackerIdGenerator' has been registered. You must either add the tracker ID services using the 'AddCommunicationTracker' extension method or you must register a suitable provider using the 'ITrackerBuilder'.");
            }

            var hasTrackerIdHeader = context.Request.Headers.TryGetValue(_options.RequestHeaderName, out var cid) && !StringValues.IsNullOrEmpty(cid);

            if (!hasTrackerIdHeader && _options.EnforceHeader)
            {
                LogHelper.EnforcedTrackerIdHeaderMissing(_logger);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(
                    $"The '{_options.RequestHeaderName}' request header is required, but was not found.");
                return;
            }

            var trackerId = hasTrackerIdHeader ? cid.FirstOrDefault() : null;

            if (hasTrackerIdHeader)
            {
                LogHelper.FoundTrackerIdHeader(_logger, trackerId);
            }
            else
            {
                LogHelper.MissingTrackerIdHeader(_logger);
            }

            if (_options.IgnoreRequestHeader || RequiresGenerationOfTrackerId(hasTrackerIdHeader, cid))
            {
                trackerId = GenerateTrackerId(context);
            }

            if (!string.IsNullOrEmpty(trackerId) && _options.UpdateTraceIdentifier)
            {
                LogHelper.UpdatingTraceIdentifier(_logger);

                context.TraceIdentifier = trackerId;
            }

            LogHelper.CreatingTrackerContext(_logger);

            trackerContextFactory.Create(trackerId, _options.RequestHeaderName);

            if (_options.IncludeInResponse && !string.IsNullOrEmpty(trackerId))
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.Headers.ContainsKey(_options.ResponseHeaderName))
                    {
                        return Task.CompletedTask;
                    }

                    LogHelper.WritingTrackerIdResponseHeader(_logger, _options.ResponseHeaderName, trackerId);
                    context.Response.Headers.Add(_options.ResponseHeaderName, trackerId);

                    return Task.CompletedTask;
                });
            }

            if (_options.AddToLogger && !string.IsNullOrEmpty(_options.LoggerScopeName) && !string.IsNullOrEmpty(trackerId))
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    [_options.LoggerScopeName] = trackerId
                }))
                {
                    LogHelper.TrackerIdProcessingEnd(_logger, trackerId);
                    await _next(context);
                }
            }
            else
            {
                LogHelper.TrackerIdProcessingEnd(_logger, trackerId);
                await _next(context);
            }

            LogHelper.DisposingTrackerContext(_logger);
            trackerContextFactory.Dispose();
        }

        private static bool RequiresGenerationOfTrackerId(bool idInHeader, StringValues idFromHeader)
            => !idInHeader || StringValues.IsNullOrEmpty(idFromHeader);

        private string GenerateTrackerId(HttpContext ctx)
        {
            string trackerId;

            if (_options.TrackerIdGenerator is { })
            {
                trackerId = _options.TrackerIdGenerator();
                LogHelper.GeneratedHeaderUsingGeneratorFunction(_logger, trackerId);
                return trackerId;
            }

            trackerId = _trackerIdGenerator.GenerateTrackerId(ctx);
            LogHelper.GeneratedHeaderUsingProvider(_logger, trackerId, _trackerIdGenerator.GetType());
            return trackerId;
        }

    }
}
