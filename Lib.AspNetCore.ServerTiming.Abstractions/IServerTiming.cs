using System.Collections.Generic;
using Lib.AspNetCore.ServerTiming.Abstractions.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Abstractions
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
