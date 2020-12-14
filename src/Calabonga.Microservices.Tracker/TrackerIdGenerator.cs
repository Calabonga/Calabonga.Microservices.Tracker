using System;
using Calabonga.Microservices.Tracker.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Calabonga.Microservices.Tracker
{
    /// <summary>
    /// Default Tracker generator for Id
    /// </summary>
    public class DefaultTrackerIdGenerator : ITrackerIdGenerator
    {
        /// <summary>
        /// Generates a tracker ID string for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> of the current request.</param>
        /// <returns>A string representing the tracker ID.</returns>
        public string GenerateTrackerId(HttpContext context)
        {
            return Guid.NewGuid().ToString();
        }
    }
}