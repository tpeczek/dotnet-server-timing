using System;
using System.Collections.Generic;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// The <see cref="IApplicationBuilder"/> extensions for adding Server Timing API middleware to pipeline.
    /// </summary>
    public static class ServerTimingMiddlewareExtensions
    {
        #region Methods
        /// <summary>
        /// Adds a <see cref="ServerTimingMiddleware"/> to application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> passed to Configure method.</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ServerTimingMiddleware>();
        }

        /// <summary>
        /// Adds a <see cref="ServerTimingMiddleware"/> to application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> passed to Configure method.</param>
        /// <param name="timingAllowOrigins">The collection of origins that are allowed to see values from timing APIs.</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder app, ICollection<string> timingAllowOrigins)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ServerTimingMiddleware>(timingAllowOrigins);
        }

        /// <summary>
        /// Adds a <see cref="ServerTimingMiddleware"/> to application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> passed to Configure method.</param>
        /// <param name="timingAllowOrigins">The origins that are allowed to see values from timing APIs.</param>
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder app, params string[] timingAllowOrigins)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ServerTimingMiddleware>(new TimingAllowOriginHeaderValue(timingAllowOrigins));
        }
        #endregion
    }
}
