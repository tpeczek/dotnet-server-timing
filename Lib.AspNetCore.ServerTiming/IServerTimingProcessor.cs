using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// Provides a processor which can inspect and modify the metrics which are to be delivered in a response to current request.
    /// </summary>
    public interface IServerTimingProcessor
    {
        /// <summary>
        /// Inspects and modifies the collection of metrics for current request. If no more metrics remain in the collection after processing, the Server-Timing header will not be sent.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="metrics">The collection of metrics for current request.</param>
        /// <returns>True if subsequent processors are allowed to run; otherwise, false.</returns>
        public bool Process(HttpContext context, ICollection<ServerTimingMetric> metrics);
    }
}
