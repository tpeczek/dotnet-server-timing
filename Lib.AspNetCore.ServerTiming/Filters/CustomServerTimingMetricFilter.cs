using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Abstractions.Filters;
using Lib.AspNetCore.ServerTiming.Abstractions.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Filters
{
    /// <summary>
    /// A function based filter that can inspect and modify the metrics which are to be delivered in a response to current request.
    /// </summary>
    public class CustomServerTimingMetricFilter : IServerTimingMetricFilter
    {
        private readonly Func<HttpContext, ICollection<ServerTimingMetric>, bool> _filter;

        /// <summary>
        /// Instantiates a new <see cref="CustomServerTimingMetricFilter"/>.
        /// </summary>
        /// <param name="filter">The function used for inspecting and modifying the collection of metrics for current request.</param>
        public CustomServerTimingMetricFilter(Func<HttpContext, ICollection<ServerTimingMetric>, bool> filter)
        {
            _filter = filter;
        }

        ///<inheritdoc/>
        public bool OnServerTimingHeaderPreparation(HttpContext context, ICollection<ServerTimingMetric> metrics) => _filter(context, metrics);
    }
}
