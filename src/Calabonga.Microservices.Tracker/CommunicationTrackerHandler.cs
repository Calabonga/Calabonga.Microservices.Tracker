using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.Microservices.Tracker.Abstractions;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> which adds the tracker ID header from the <see cref="TrackerContext"/> onto outgoing HTTP requests.
    /// </summary>
    internal sealed class CommunicationTrackerHandler : DelegatingHandler
    {
        private readonly ITrackerContextAccessor _trackerContextAccessor;

        public CommunicationTrackerHandler(ITrackerContextAccessor trackerContextAccessor) => _trackerContextAccessor = trackerContextAccessor;

        /// <inheritdoc cref="DelegatingHandler"/>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_trackerContextAccessor.TrackerContext == null)
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (!request.Headers.Contains(_trackerContextAccessor.TrackerContext.Header))
            {
                request.Headers.Add(_trackerContextAccessor.TrackerContext.Header, _trackerContextAccessor.TrackerContext.TrackerId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
