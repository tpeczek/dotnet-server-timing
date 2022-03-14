using System.Collections.Generic;

namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// Configuration options for server timing
    /// </summary>
    public class ServerTimingOptions
    {
        /// <summary>
        /// The collection of origins that are allowed to see values from timing APIs.
        /// </summary>
        public List<string> AllowedOrigins { get; } = new List<string>();

        /// <summary>
        /// The collection of processors that are executed before any header is sent
        /// </summary>
        public List<IServerTimingProcessor> Processors { get; } = new List<IServerTimingProcessor>();
    }
}
