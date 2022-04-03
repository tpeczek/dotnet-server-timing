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

            return UseServerTiming(app, options=> { });
        }

        /// <summary>
        /// Adds a <see cref="ServerTimingMiddleware"/> to application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> passed to Configure method.</param>
        /// <param name="timingAllowOrigins">The collection of origins that are allowed to see values from timing APIs.</param>
        /// <returns>The original app parameter</returns>
        [Obsolete("Use a version of this method that takes a lambda function to intialise options")]
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder app, ICollection<string> timingAllowOrigins)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return UseServerTiming(app, options => options.AllowedOrigins.AddRange(timingAllowOrigins));
        }

        /// <summary>
        /// Adds a <see cref="ServerTimingMiddleware"/> to application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> passed to Configure method.</param>
        /// <param name="timingAllowOrigins">The origins that are allowed to see values from timing APIs.</param>
        /// <returns>The original app parameter</returns>
        [Obsolete("Use a version of this method that takes a lambda function to intialise options")]
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder app, params string[] timingAllowOrigins)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return UseServerTiming(app,options => options.AllowedOrigins.AddRange(timingAllowOrigins));
        }


        /// <summary>
        /// Adds a <see cref="ServerTimingMiddleware"/> to application pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> passed to Configure method.</param>
        /// <param name="setOptionsCallback">A lambda to set the configuration options for server timing</param>        
        /// <returns>The original app parameter</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder app, Action<ServerTimingOptions> setOptionsCallback)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            var options = new ServerTimingOptions();
            setOptionsCallback?.Invoke(options);

            return app.UseMiddleware<ServerTimingMiddleware>(options);
        }
        #endregion
    }
}
