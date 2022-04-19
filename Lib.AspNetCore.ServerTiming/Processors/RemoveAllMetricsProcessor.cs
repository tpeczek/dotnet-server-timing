using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Lib.AspNetCore.ServerTiming.Processors
{
    /// <summary>
    /// A processor to clear all metrics in the response
    /// </summary>
    public class RemoveAllMetricsProcessor : IServerTimingProcessor
    {
        /// <inheritdoc/>
        public bool Process(HttpContext context, ICollection<ServerTimingMetric> metrics)
        {
            metrics.Clear();
            return true;
        }
    }
}
