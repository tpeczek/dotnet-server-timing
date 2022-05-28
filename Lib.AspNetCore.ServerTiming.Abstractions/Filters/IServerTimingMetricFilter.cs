using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Abstractions.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Abstractions.Filters
{
    /// <summary>
    /// A filter that can inspect and modify the metrics which are to be delivered in a response to current request.
    /// </summary>
    public interface IServerTimingMetricFilter
    {
        /// <summary>
        /// Inspects and modifies the collection of metrics for current request. If no more metrics remain in the collection after, the Server-Timing header will not be sent.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="metrics">The collection of metrics for current request.</param>
        /// <returns>True if subsequent filters are allowed to run, otherwise false.</returns>
        public bool OnServerTimingHeaderPreparation(HttpContext context, ICollection<ServerTimingMetric> metrics);
    }
}
