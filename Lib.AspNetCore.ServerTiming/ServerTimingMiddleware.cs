﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Lib.AspNetCore.ServerTiming.Http.Extensions;
using System.Linq;

namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// Middleware providing support for Server Timing API.
    /// </summary>
    public class ServerTimingMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private readonly string _timingAllowOriginHeaderValue;
        private readonly List<IServerTimingProcessor> _processors;
        private static Task _completedTask = Task.FromResult<object>(null);
        #endregion

        #region Constructor
        /// <summary>
        /// Instantiates a new <see cref="ServerTimingMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public ServerTimingMiddleware(RequestDelegate next)
            : this(next, (TimingAllowOriginHeaderValue)null)
        { }

        /// <summary>
        /// Instantiates a new <see cref="ServerTimingMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="timingAllowOrigins">The collection of origins that are allowed to see values from timing APIs.</param>
        public ServerTimingMiddleware(RequestDelegate next, ICollection<string> timingAllowOrigins)
            : this(next, new TimingAllowOriginHeaderValue(timingAllowOrigins))
        { }

        /// <summary>
        /// Instantiates a new <see cref="ServerTimingMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="timingAllowOrigin">The Timing-Allow-Origin header value.</param>
        public ServerTimingMiddleware(RequestDelegate next, TimingAllowOriginHeaderValue timingAllowOrigin)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _timingAllowOriginHeaderValue = timingAllowOrigin?.ToString();
            _processors = new List<IServerTimingProcessor>();
        }

        /// <summary>
        /// Instantiates a new <see cref="ServerTimingMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="options">Server timing configuration options</param>        
        public ServerTimingMiddleware(RequestDelegate next, ServerTimingOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _timingAllowOriginHeaderValue = new TimingAllowOriginHeaderValue(options.AllowedOrigins)?.ToString();
            _processors = options.Processors;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serverTiming">The instance of <see cref="IServerTiming"/> for current requet.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task Invoke(HttpContext context, IServerTiming serverTiming)
        {
            if (serverTiming is null)
            {
                throw new ArgumentNullException(nameof(serverTiming));
            }

            if (_timingAllowOriginHeaderValue != null)
            {
                context.Response.SetResponseHeader(HeaderNames.TimingAllowOrigin, _timingAllowOriginHeaderValue);
            }

            return HandleServerTimingAsync(context, serverTiming);
        }

#if !NETCOREAPP2_1 && !NET461
        private async Task HandleServerTimingAsync(HttpContext context, IServerTiming serverTiming)
        {
            if (context.Request.AllowsTrailers() && context.Response.SupportsTrailers())
            {
                await HandleServerTimingAsTrailerHeaderAsync(context, serverTiming);
            }
            else
            {
                await HandleServerTimingAsResponseHeaderAsync(context, serverTiming);
            }
        }

        private async Task HandleServerTimingAsTrailerHeaderAsync(HttpContext context, IServerTiming serverTiming)
        {
            context.Response.DeclareTrailer(HeaderNames.ServerTiming);

            serverTiming.SetServerTimingDeliveryMode(ServerTimigDeliveryMode.Trailer);

            await _next(context);

            var metrics = RunProcessors(serverTiming,context);
            if (metrics.Any())
            {            
                context.Response.SetServerTimingTrailer(new ServerTimingHeaderValue(metrics));
            }
        }
#else
        private Task HandleServerTimingAsync(HttpContext context, IServerTiming serverTiming)
        {
            return HandleServerTimingAsResponseHeaderAsync(context, serverTiming);
        }
#endif

        private Task HandleServerTimingAsResponseHeaderAsync(HttpContext context, IServerTiming serverTiming)
        {
            context.Response.OnStarting(() => {
                var metrics = RunProcessors(serverTiming,context);
                if (metrics.Any())
                {
                    context.Response.SetServerTiming(new ServerTimingHeaderValue(metrics));
                }

                return _completedTask;
            });
            
            serverTiming.SetServerTimingDeliveryMode(ServerTimigDeliveryMode.ResponseHeader);

            return _next(context);
        }

        private List<ServerTimingMetric> RunProcessors(IServerTiming serverTiming, HttpContext context)
        {
            //Create a mutable copy of metrics
            var metrics = new List<ServerTimingMetric>(serverTiming.Metrics);

            //Allow processors to filter them
            foreach(var processor in _processors)
            {
                if (!processor.Process(context, metrics)) break;
            }
            return metrics;
        }
        #endregion
    }
}
