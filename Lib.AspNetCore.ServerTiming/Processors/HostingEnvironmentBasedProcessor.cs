using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Processors
{
    /// <summary>
    /// Base class for processors whose behavior depends on the web hosting environment an application is running in.
    /// </summary>
    public abstract class HostingEnvironmentBasedProcessor : IServerTimingProcessor
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

#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Instantiates a new <see cref="HostingEnvironmentBasedProcessor"/>.
        /// </summary>
        /// <param name="hostEnvironment">The <see cref="Microsoft.Extensions.Hosting.IHostEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public HostingEnvironmentBasedProcessor(Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            IsDevelopment = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Development;
            IsStaging = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Staging;
            IsProduction = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Production;
        }
#else
        /// <summary>
        /// Instantiates a new <see cref="HostingEnvironmentBasedProcessor"/>.
        /// </summary>
        /// <param name="hostingEnvironment">The <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public HostingEnvironmentBasedProcessor(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            IsDevelopment = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsDevelopment(hostingEnvironment);
            IsStaging = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsStaging(hostingEnvironment);
            IsProduction = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsProduction(hostingEnvironment);
        }
#endif

        /// <inheritdoc/>
        public abstract bool Process(HttpContext context, ICollection<ServerTimingMetric> metrics);
    }
}
