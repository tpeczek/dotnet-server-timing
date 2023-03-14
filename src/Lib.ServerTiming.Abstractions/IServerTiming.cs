using System.Collections.Generic;
using Lib.ServerTiming.Http.Headers;

namespace Lib.ServerTiming
{
    /// <summary>
    /// Provides support for Server Timing API.
    /// </summary>
    public interface IServerTiming
    {
        /// <summary>
        /// Gets the metrics delivery mode for current request.
        /// </summary>
        ServerTimingDeliveryMode DeliveryMode { get; }

        /// <summary>
        /// Gets the collection of metrics for current request.
        /// </summary>
        ICollection<ServerTimingMetric> Metrics { get; }        
    }
}
