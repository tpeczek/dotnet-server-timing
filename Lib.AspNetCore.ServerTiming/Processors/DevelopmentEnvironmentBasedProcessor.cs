using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Lib.AspNetCore.ServerTiming.Processors
{
    /// <summary>
    /// Base class for processors whose behaviour changes in the development environment
    /// </summary>
    public abstract class DevelopmentEnvironmentBasedProcessor : IServerTimingProcessor
    {
        /// <summary>
        /// True if running in the development environment
        /// </summary>
        protected readonly bool IsDevelopment = false;


#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Create an DevelopmentEnvironmentBasedProcessor
        /// </summary>
        /// <param name="hostEnvironment">The host environment used to determine if this is a development environment</param>
        public DevelopmentEnvironmentBasedProcessor(Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            IsDevelopment = hostEnvironment.EnvironmentName == Microsoft.Extensions.Hosting.Environments.Development;
        }
#else
        /// <summary>
        /// Create an AllowInDevelopmentEnvironmentProcessor
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment used to determine if this is a development environment</param>
        public DevelopmentEnvironmentBasedProcessor(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            IsDevelopment = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions.IsDevelopment(hostingEnvironment);
        }
#endif
        /// <inheritdoc/>
        public abstract bool Process(HttpContext context, List<ServerTimingMetric> metrics);
    }
}
