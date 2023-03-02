using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Lib.Azure.Functions.Worker.ServerTiming;

namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// The <see cref="IFunctionsWorkerApplicationBuilder"/> extensions for adding <see cref="ServerTimingMiddleware"/> to the worker execution pipeline.
    /// </summary>
    public static class ServerTimingMiddlewareExtensions
    {
        #region Methods
        /// <summary>
        /// Configures the <see cref="IFunctionsWorkerApplicationBuilder"/> to use <see cref="ServerTimingMiddleware"/> in the worker execution pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IFunctionsWorkerApplicationBuilder"/> to configure.</param>
        /// <returns>The same instance of the <see cref="IFunctionsWorkerApplicationBuilder"/> for chaining.</returns>
        public static IFunctionsWorkerApplicationBuilder UseServerTiming(this IFunctionsWorkerApplicationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddSingleton(new ServerTimingOptions());

            return builder.UseMiddleware<ServerTimingMiddleware>();
        }

        /// <summary>
        /// Configures the <see cref="IFunctionsWorkerApplicationBuilder"/> to use <see cref="ServerTimingMiddleware"/> in the worker execution pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IFunctionsWorkerApplicationBuilder"/> to configure.</param>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to configure the middleware.</param>        
        /// <returns>The same instance of the <see cref="IFunctionsWorkerApplicationBuilder"/> for chaining.</returns>
        public static IFunctionsWorkerApplicationBuilder UseServerTiming(this IFunctionsWorkerApplicationBuilder builder, ServerTimingOptions options)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            builder.Services.AddSingleton(options);

            return builder.UseMiddleware<ServerTimingMiddleware>();
        }
        #endregion
    }
}
