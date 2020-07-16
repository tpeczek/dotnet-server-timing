using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Lib.AspNetCore.ServerTiming.Http.Extensions;

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

            HandleServerTiming(context, serverTiming);

            return _next(context);
        }

        private void HandleServerTiming(HttpContext context, IServerTiming serverTiming)
        {
            context.Response.OnStarting(() => {
                if (serverTiming.Metrics.Count > 0)
                {
                    context.Response.SetServerTiming(new ServerTimingHeaderValue(serverTiming.Metrics));
                }

                return _completedTask;
            });
        }
        #endregion
    }
}
