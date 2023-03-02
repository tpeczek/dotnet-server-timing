using System.Collections.Generic;

namespace Lib.Azure.Functions.Worker.ServerTiming
{
    /// <summary>
    /// Configuration options for server timing
    /// </summary>
    public class ServerTimingOptions
    {
        /// <summary>
        /// The collection of origins that are allowed to see values from timing APIs.
        /// </summary>
        public List<string> AllowedOrigins { get; set; } = new List<string>();
    }
}
