using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Filters
{
    /// <summary>
    /// A filter which will remove all metrics unless an application is running in development environment.
    /// </summary>
    public class RestrictToDevelopmentMetricFilter : HostingEnvironmentServerTimingMetricFilter
    {
#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Instantiates a new <see cref="RestrictToDevelopmentMetricFilter"/>.
        /// </summary>
        /// <param name="hostEnvironment">The <see cref="Microsoft.Extensions.Hosting.IHostEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public RestrictToDevelopmentMetricFilter(Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment) : base(hostEnvironment)
        { }
#else
        /// <summary>
        /// Instantiates a new <see cref="RestrictToDevelopmentMetricFilter"/>.
        /// </summary>
        /// <param name="hostingEnvironment">The <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public RestrictToDevelopmentMetricFilter(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) : base(hostingEnvironment)
        { }
#endif

        /// <summary>
        /// Removes all metrics for current request unless an application is running in development environment.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="metrics">The collection of metrics for current request.</param>
        /// <returns>True if subsequent filters are allowed to run, otherwise false.</returns>
        public override bool OnServerTimingHeaderPreparation(HttpContext context, ICollection<ServerTimingMetric> metrics)
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
