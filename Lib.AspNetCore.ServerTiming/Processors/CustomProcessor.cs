using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Lib.AspNetCore.ServerTiming.Processors
{
    /// <summary>
    /// Implement a custom processor using a lambda
    /// </summary>
    public class CustomProcessor : IServerTimingProcessor
    {
        private readonly Func<HttpContext, List<ServerTimingMetric>, bool> _process;

        /// <summary>
        /// Create a custom processor
        /// </summary>
        /// <param name="process">Lambda to execute processing</param>
        public CustomProcessor(Func<HttpContext, List<ServerTimingMetric>, bool> process)
        {
            _process = process;
        }

        ///<inheritdoc/>
        public bool Process(HttpContext context, List<ServerTimingMetric> metrics) => _process(context, metrics);
    }
}
