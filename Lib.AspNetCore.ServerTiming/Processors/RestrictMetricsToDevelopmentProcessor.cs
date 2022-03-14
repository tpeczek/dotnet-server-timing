using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Lib.AspNetCore.ServerTiming.Processors
{
    /// <summary>
    /// A processor which will allow all headers to be sent in the development environment,
    /// and will not run any futher processors
    /// </summary>
    public class RestrictMetricsToDevelopmentProcessor : DevelopmentEnvironmentBasedProcessor
    {
#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Create an RestrictMetricsToDevelopmentProcessor
        /// </summary>
        /// <param name="hostEnvironment">The host environment used to determine if this is a development environment</param>
        public RestrictMetricsToDevelopmentProcessor(Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment):base(hostEnvironment)
        {
        }
#else
        /// <summary>
        /// Create an RestrictMetricsToDevelopmentProcessor
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment used to determine if this is a development environment</param>
        public RestrictMetricsToDevelopmentProcessor(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        {
        }
#endif

        /// <summary>
        /// Force no further middleware to run if in development environment
        /// </summary>
        /// <param name="context">Not used</param>
        /// <param name="metrics">Not used</param>
        /// <returns></returns>
        public override bool Process(HttpContext context, List<ServerTimingMetric> metrics)
        {
            if (!IsDevelopment)
            {
                metrics.Clear();
                return false;
            }
            return true;
        }
    }
}
