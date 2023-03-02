﻿using Lib.ServerTiming;
using Lib.Azure.Functions.Worker.ServerTiming;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// The <see cref="IServiceCollection"/> extensions for adding Server Timing API related services.
    /// </summary>
    public static class ServerTimingServiceCollectionExtensions
    {
        #region Methods
        /// <summary>
        /// Registers default service which provides support for Server Timing API.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        /// <returns>The collection of service descriptors.</returns>
        public static IServiceCollection AddServerTiming(this IServiceCollection services)
        {
            services.AddScoped<IServerTiming, ServerTiming>();

            return services;
        }
        #endregion
    }
}
