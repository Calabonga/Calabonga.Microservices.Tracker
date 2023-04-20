using System;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// Microservice request and response tracker options
    /// </summary>
    public sealed class TrackerOptions
    {
        private string _responseHeader;

        /// <summary>
        /// The default header used for tracker ID.
        /// </summary>
        public const string DefaultHeaderName = "X-Trace-Id";

        /// <summary>
        /// The default logger scope key for tracker ID logging.
        /// </summary>
        public const string LoggerKey = "TrackerId";

        /// <summary>
        /// The name of the header from which the tracker ID is read.
        /// </summary>
        public string RequestHeaderName { get; set; } = DefaultHeaderName;

        /// <summary>
        /// The name of the header from which the tracker ID is written.
        /// </summary>
        public string ResponseHeaderName { get => _responseHeader ?? RequestHeaderName; set => _responseHeader = value; }

        /// <summary>
        /// <para>
        /// Ignore request header.
        /// When true the TraceIdentifier for the current request ignores the header from request.
        /// </para>
        /// <para>Default: false</para>
        /// </summary>
        public bool IgnoreRequestHeader { get; set; } = false;

        /// <summary>
        /// <para>
        /// Enforce the inclusion of the tracker ID request header.
        /// When true and a tracker ID header is not included, the request will fail with a 400 Bad Request response.
        /// </para>
        /// <para>Default: false</para>
        /// </summary>
        public bool EnforceHeader { get; set; } = false;

        /// <summary>
        /// <para>
        /// Add the tracker ID value to the logger scope for all requests.
        /// When true the value of the tracker ID will be added to the logger scope payload.
        /// </para>
        /// <para>Default: false</para>
        /// </summary>
        public bool AddToLogger { get; set; } = false;

        /// <summary>
        /// <para>
        /// The name for the key used when adding the tracker ID to the logger scope.
        /// </para>
        /// <para>Default: 'trackerId'</para>
        /// </summary>
        public string LoggerScopeName { get; set; } = LoggerKey;

        /// <summary>
        /// <para>
        /// Controls whether the tracker ID is returned in the response headers.
        /// </para>
        /// <para>Default: true</para>
        /// </summary>
        public bool IncludeInResponse { get; set; } = true;

        /// <summary>
        /// <para>
        /// Controls whether the ASP.NET Core TraceIdentifier will be set to match the trackerId.
        /// </para>
        /// <para>Default: false</para>
        /// </summary>
        public bool UpdateTraceIdentifier { get; set; } = false;

        /// <summary>
        /// A function that returns the tracker ID in cases where no tracker ID is retrieved from the request header. It can be used to customise the tracker ID generation.
        /// When set, this function will be used instead of the registered <see cref="ITrackerIdProvider"/>.
        /// </summary>
        public Func<string> TrackerIdGenerator { get; set; }
    }
}
