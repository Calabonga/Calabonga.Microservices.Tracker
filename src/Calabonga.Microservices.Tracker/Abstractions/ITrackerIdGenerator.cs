using Microsoft.AspNetCore.Http;

namespace Calabonga.Microservices.Tracker.Abstractions
{
    /// <summary>
    /// Tracker request identifier generator
    /// </summary>
    public interface ITrackerIdGenerator
    {
        /// <summary>
        /// Generates a tracker ID string for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> of the current request.</param>
        /// <returns>A string representing the tracker ID.</returns>
        string GenerateTrackerId(HttpContext context);
    }
}
