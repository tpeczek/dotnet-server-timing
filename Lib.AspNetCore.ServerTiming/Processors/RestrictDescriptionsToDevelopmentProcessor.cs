using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Lib.AspNetCore.ServerTiming.Processors
{
    /// <summary>
    /// A processor which will remove the descriptions from all metrics unless in the development environment
    /// </summary>
    public class RestrictDescriptionsToDevelopmentProcessor : DevelopmentEnvironmentBasedProcessor
    {
#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Create an RestrictDescriptionsToDevelopmentProcessor
        /// </summary>
        /// <param name="hostEnvironment">The host environment used to determine if this is a development environment</param>
        public RestrictDescriptionsToDevelopmentProcessor(Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment):base(hostEnvironment)
        {
        }
#else
        /// <summary>
        /// Create an RestrictDescriptionsToDevelopmentProcessor
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment used to determine if this is a development environment</param>
        public RestrictDescriptionsToDevelopmentProcessor(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
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
                List<ServerTimingMetric> newMetrics = metrics.Select(m => new ServerTimingMetric(m.Name,m.Value,null)).ToList();
                metrics.Clear();
                metrics.AddRange(newMetrics);
            }
            //Allow next processor to run
            return true;
        }
    }
}
