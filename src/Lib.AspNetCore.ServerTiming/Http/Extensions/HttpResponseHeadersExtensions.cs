using System;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Http.Extensions
{
    /// <summary>
    /// Extensions for setting timing APIs response headers.
    /// </summary>
    public static class HttpResponseHeadersExtensions
    {
        #region Methods
        /// <summary>
        /// Sets the Server-Timing header value.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="serverTiming">The Server-Timing header value.</param>
        public static void SetServerTiming(this HttpResponse response, ServerTimingHeaderValue serverTiming)
        {
            response.SetResponseHeader(HeaderNames.ServerTiming, serverTiming?.ToString());
        }

        /// <summary>
        /// Sets the Server-Timing header value.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="metrics">The metrics.</param>
        public static void SetServerTiming(this HttpResponse response, params ServerTimingMetric[] metrics)
        {
            ServerTimingHeaderValue serverTiming = new ServerTimingHeaderValue(metrics);

            response.SetResponseHeader(HeaderNames.ServerTiming, serverTiming.ToString());
        }

#if !NETCOREAPP2_1 && !NET462
        /// <summary>
        /// Sets the Server-Timing trailer header value.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="serverTiming">The Server-Timing header value.</param>
        public static void SetServerTimingTrailer(this HttpResponse response, ServerTimingHeaderValue serverTiming)
        {
            response.AppendTrailer(HeaderNames.ServerTiming, serverTiming?.ToString());
        }

        /// <summary>
        /// Sets the Server-Timing trailer header value.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="metrics">The metrics.</param>
        public static void SetServerTimingTrailer(this HttpResponse response, params ServerTimingMetric[] metrics)
        {
            ServerTimingHeaderValue serverTiming = new ServerTimingHeaderValue(metrics);

            response.AppendTrailer(HeaderNames.ServerTiming, serverTiming.ToString());
        }
#endif

        /// <summary>
        /// Sets the Timing-Allow-Origin header value.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="timingAllowOrigin">The Timing-Allow-Origin header value.</param>
        public static void SetTimingAllowOrigin(this HttpResponse response, TimingAllowOriginHeaderValue timingAllowOrigin)
        {
            response.SetResponseHeader(HeaderNames.TimingAllowOrigin, timingAllowOrigin?.ToString());
        }

        /// <summary>
        /// Sets the Timing-Allow-Origin header value.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="origins">The origins that are allowed to see values from timing APIs.</param>
        public static void SetTimingAllowOrigin(this HttpResponse response, params string[] origins)
        {
            TimingAllowOriginHeaderValue timingAllowOrigin = new TimingAllowOriginHeaderValue(origins);

            response.SetResponseHeader(HeaderNames.TimingAllowOrigin, timingAllowOrigin.ToString());
        }

        internal static void SetResponseHeader(this HttpResponse response, string headerName, string headerValue)
        {
            if (!String.IsNullOrWhiteSpace(headerValue))
            {
                if (response.Headers.ContainsKey(headerName))
                {
                    response.Headers[headerName] = headerValue;
                }
                else
                {
                    response.Headers.Append(headerName, headerValue);
                }
            }
        }
        #endregion
    }
}
