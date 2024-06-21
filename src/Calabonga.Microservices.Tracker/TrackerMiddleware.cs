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
        private readonly TrackerOptions _trackerOptions;
        private readonly ExcludeOptions _excludeOptions;

        /// <summary>
        /// Creates a new instance of the TrackerMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The <see cref="ILogger"/> instance to log to.</param>
        /// <param name="trackerOptions">The configuration trackerOptions.</param>
        /// <param name="excludeOptions"></param>
        /// <param name="trackerIdGenerator"></param>
        public TrackerMiddleware(
            RequestDelegate next,
            ILogger<TrackerMiddleware> logger,
            IOptions<TrackerOptions> trackerOptions,
            IOptions<ExcludeOptions> excludeOptions,
            ITrackerIdGenerator trackerIdGenerator = null)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _trackerIdGenerator = trackerIdGenerator;
            _trackerOptions = trackerOptions.Value;
            _excludeOptions = excludeOptions.Value;
        }

        /// <summary>
        /// Processes a request to synchronise TraceIdentifier and Tracker ID headers. Also creates a 
        /// <see cref="TrackerContext"/> for the current request and disposes of it when the request is completing.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="trackerContextFactory">The <see cref="ITrackerContextFactory"/> which can create a <see cref="TrackerContext"/>.</param>
        public async Task Invoke(HttpContext context, ITrackerContextFactory trackerContextFactory)
        {
            if (await IsNeedToSkip(context))
            {
                return;
            }

            LogHelper.TrackerIdProcessingBegin(_logger);

            if (_trackerIdGenerator is null)
            {
                LogHelper.MissingTrackerIdProvider(_logger);

                throw new InvalidOperationException("No 'ITrackerIdGenerator' has been registered. You must either add the tracker ID services using the 'AddCommunicationTracker' extension method or you must register a suitable provider using the 'ITrackerBuilder'.");
            }

            var hasTrackerIdHeader = context.Request.Headers.TryGetValue(_trackerOptions.RequestHeaderName, out var cid) && !StringValues.IsNullOrEmpty(cid);

            if (!hasTrackerIdHeader && _trackerOptions.EnforceHeader)
            {
                LogHelper.EnforcedTrackerIdHeaderMissing(_logger);

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync($"The '{_trackerOptions.RequestHeaderName}' request header is required, but was not found.");
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

            if (_trackerOptions.IgnoreRequestHeader || RequiresGenerationOfTrackerId(hasTrackerIdHeader, cid))
            {
                trackerId = GenerateTrackerId(context);
            }

            if (!string.IsNullOrEmpty(trackerId) && _trackerOptions.UpdateTraceIdentifier)
            {
                LogHelper.UpdatingTraceIdentifier(_logger);

                context.TraceIdentifier = trackerId;
            }

            LogHelper.CreatingTrackerContext(_logger);

            trackerContextFactory.Create(trackerId, _trackerOptions.RequestHeaderName);

            if (_trackerOptions.IncludeInResponse && !string.IsNullOrEmpty(trackerId))
            {
                context.Response.OnStarting(async () =>
                {
                    if (context.Response.Headers.ContainsKey(_trackerOptions.ResponseHeaderName))
                    {
                        return;
                    }

                    if (await IsNeedToSkip(context))
                    {
                        return;
                    }

                    LogHelper.WritingTrackerIdResponseHeader(_logger, _trackerOptions.ResponseHeaderName, trackerId);
                    context.Response.Headers.Add(_trackerOptions.ResponseHeaderName, trackerId);
                });
            }

            if (_trackerOptions.AddToLogger && !string.IsNullOrEmpty(_trackerOptions.LoggerScopeName) && !string.IsNullOrEmpty(trackerId))
            {
                using (_logger.BeginScope(new Dictionary<string, object>
                {
                    [_trackerOptions.LoggerScopeName] = trackerId
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

        private async Task<bool> IsNeedToSkip(HttpContext context)
        {
            if (_excludeOptions.CheckSchemeContainsValue(context.Request.Scheme))
            {
                LogHelper.TrackerIdExcludesSchemeProcessing(_logger, context.Request.Scheme);
                await _next(context);
                return true;
            }

            if (context.Request.Host.HasValue && _excludeOptions.CheckHostContainsValue(context.Request.Host.Value))
            {
                LogHelper.TrackerIdExcludesHostProcessing(_logger, context.Request.Host.Value);
                await _next(context);
                return true;
            }

            if (!context.Request.Path.HasValue || !_excludeOptions.CheckPathContainsValue(context.Request.Path))
            {
                return false;
            }

            LogHelper.TrackerIdExcludesPathProcessing(_logger, context.Request.Path);
            await _next(context);
            return true;

        }

        private static bool RequiresGenerationOfTrackerId(bool idInHeader, StringValues idFromHeader)
            => !idInHeader || StringValues.IsNullOrEmpty(idFromHeader);

        private string GenerateTrackerId(HttpContext ctx)
        {
            string trackerId;

            if (_trackerOptions.TrackerIdGenerator is { })
            {
                trackerId = _trackerOptions.TrackerIdGenerator();
                LogHelper.GeneratedHeaderUsingGeneratorFunction(_logger, trackerId);
                return trackerId;
            }

            trackerId = _trackerIdGenerator.GenerateTrackerId(ctx);
            LogHelper.GeneratedHeaderUsingProvider(_logger, trackerId, _trackerIdGenerator.GetType());
            return trackerId;
        }

    }
}
