using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.ServerTiming.Filters;
using Lib.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Filters
{
    /// <summary>
    /// Base class for filters whose behavior depends on the web hosting environment an application is running in.
    /// </summary>
    public abstract class HostingEnvironmentServerTimingMetricFilter : IServerTimingMetricFilter<HttpContext>
    {
        /// <summary>
        /// True if the environment name is development, otherwise false.
        /// </summary>
        protected bool IsDevelopment { get; }

        /// <summary>
        /// True if the environment name is staging, otherwise false.
        /// </summary>
        protected bool IsStaging { get; }

        /// <summary>
        /// True if the environment name is production, otherwise false.
        /// </summary>
        protected bool IsProduction { get; }

#if !NETCOREAPP2_1 && !NET462
        /// <summary>
        /// Instantiates a new <see cref="HostingEnvironmentServerTimingMetricFilter"/>.
        /// </summary>
        /// <param name="hostEnvironment">The <see cref="Microsoft.Extensions.Hosting.IHostEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public HostingEnvironmentServerTimingMetricFilter(Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            IsDevelopment = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Development;
            IsStaging = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Staging;
            IsProduction = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Production;
        }
#else
        /// <summary>
        /// Instantiates a new <see cref="HostingEnvironmentServerTimingMetricFilter"/>.
        /// </summary>
        /// <param name="hostingEnvironment">The <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public HostingEnvironmentServerTimingMetricFilter(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            IsDevelopment = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsDevelopment(hostingEnvironment);
            IsStaging = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsStaging(hostingEnvironment);
            IsProduction = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsProduction(hostingEnvironment);
        }
#endif

        /// <inheritdoc/>
        public abstract bool OnServerTimingHeaderPreparation(HttpContext context, ICollection<ServerTimingMetric> metrics);
    }
}
