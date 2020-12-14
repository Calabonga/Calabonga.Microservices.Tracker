using System.Threading;
using System.Threading.Tasks;
using Calabonga.microservices.Tracker.Demo.AppStart;

namespace Calabonga.microservices.Tracker.Demo.Infrastructure
{
    public class RequestProvider: IRequestProvider
    {
        private readonly IRequestService _requestService;

        public RequestProvider(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public Task<IRequestItem> SendRequest(IRequestItem item, CancellationToken token)
        {
            return _requestService.Send(item, token);
        }
    }

    public interface IRequestProvider
    {
        Task<IRequestItem> SendRequest(IRequestItem item, CancellationToken token);
    }
}
