using Calabonga.Microservices.Tracker.Abstractions;
using Calabonga.Utils.TokenGeneratorCore;
using Microsoft.AspNetCore.Http;

namespace Calabonga.microservices.Tracker.Demo
{
    public class CustomTrackerIdGenerator : ITrackerIdGenerator
    {
        /// <summary>
        /// Generates a tracker ID string for the current request.
        /// </summary>
        /// <param name="context">The <see cref="Microsoft.AspNetCore.Http.HttpContext"/> of the current request.</param>
        /// <returns>A string representing the tracker ID.</returns>
        public string GenerateTrackerId(HttpContext context)
        {
            return $"{TokenGenerator.Generate(11)}-{TokenGenerator.Generate(11)}";
        }
    }
}