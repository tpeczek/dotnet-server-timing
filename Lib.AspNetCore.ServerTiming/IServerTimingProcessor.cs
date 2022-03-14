using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// Provides an oportunity to inspect and modify the metrics about to be sent in an HTTP repsonse
    /// </summary>
    public interface IServerTimingProcessor
    {
        /// <summary>
        /// Inspects/modifies the set of metrics to be sent in an HTTP Response. If the metrics are
        /// cleared, no server timing headers are sent
        /// </summary>
        /// <param name="context">The context of the current request</param>
        /// <param name="metrics">The provisional set of metrics to be returned in the response header</param>
        /// <returns>True if subsequent processors are allowed to run, false if this should be the last processor executed</returns>
        public bool Process(HttpContext context, List<ServerTimingMetric> metrics);
    }
}
