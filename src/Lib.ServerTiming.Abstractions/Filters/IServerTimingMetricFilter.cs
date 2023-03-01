using System.Collections.Generic;
using Lib.ServerTiming.Http.Headers;

namespace Lib.ServerTiming.Filters
{
    /// <summary>
    /// A filter that can inspect and modify the metrics which are to be delivered in a response to current request.
    /// </summary>
    /// <typeparam name="TContext">The type of context required by filter.</typeparam>
    public interface IServerTimingMetricFilter<TContext>
    {
        /// <summary>
        /// Inspects and modifies the collection of metrics for current request. If no more metrics remain in the collection after, the Server-Timing header will not be sent.
        /// </summary>
        /// <param name="context">The context for the current request.</param>
        /// <param name="metrics">The collection of metrics for current request.</param>
        /// <returns>True if subsequent filters are allowed to run, otherwise false.</returns>
        /// 
        public bool OnServerTimingHeaderPreparation(TContext context, ICollection<ServerTimingMetric> metrics);
    }
}
