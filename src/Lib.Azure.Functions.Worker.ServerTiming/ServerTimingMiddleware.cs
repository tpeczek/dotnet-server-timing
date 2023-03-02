using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Lib.ServerTiming;
using Lib.ServerTiming.Http.Headers;

namespace Lib.Azure.Functions.Worker.ServerTiming
{
    /// <summary>
    /// Middleware providing support for Server Timing API in the worker execution pipeline.
    /// </summary>
    public class ServerTimingMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly string? _timingAllowOriginHeaderValue = null;

        /// <summary>
        /// Instantiates a new <see cref="ServerTimingMiddleware"/>.
        /// </summary>
        /// <param name="options">Server timing configuration options</param>        
        public ServerTimingMiddleware(ServerTimingOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if ((options.AllowedOrigins is not null) && (options.AllowedOrigins.Count > 0))
            {
                _timingAllowOriginHeaderValue = new TimingAllowOriginHeaderValue(options.AllowedOrigins).ToString();
            }
        }

        /// <summary>
        /// Process an individual invocation.
        /// </summary>
        /// <param name="context">The <see cref="FunctionContext"/> for the current invocation.</param>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            await next(context);

            HttpResponseData? response = context.GetHttpResponseData();
            if (response is not null)
            {
                if (_timingAllowOriginHeaderValue is not null)
                {
                    response.Headers.Add(HeaderNames.TimingAllowOrigin, _timingAllowOriginHeaderValue);
                }

                IServerTiming serverTiming = context.InstanceServices.GetRequiredService<IServerTiming>();
                response.Headers.Add(HeaderNames.ServerTiming, new ServerTimingHeaderValue(serverTiming.Metrics).ToString());
            }
        }
    }
}
