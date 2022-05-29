using System.Collections.Generic;
using Lib.AspNetCore.ServerTiming.Filters;

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
        public List<string> AllowedOrigins { get; set;  } = new List<string>();

        /// <summary>
        /// The collection of filters that are executed before Server-Timing header is sent.
        /// </summary>
        public List<IServerTimingMetricFilter> Filters { get; set; } = new List<IServerTimingMetricFilter>();
    }
}
