using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Filters;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// Methods to provide shortcuts for setting the server timing options
    /// </summary>
    public static class ServerTimingOptionsExtensions
    {
#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Configure the processors collection to only send headers in the development environment
        /// </summary>
        /// <param name="options">The options to modify</param>
        /// <param name="hostEnvironment">The host application's environment</param>
        public static void RestrictMetricsToDevelopment(this ServerTimingOptions options, 
            Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            options.Filters.Add(new RestrictToDevelopmentMetricFilter(hostEnvironment));
        }

        /// <summary>
        /// Configure the processors collection to only send headers in the development environment
        /// </summary>
        /// <param name="options">The options to modify</param>
        /// <param name="hostEnvironment">The host application's environment</param>
        public static void RestrictDescriptionsToDevelopment(this ServerTimingOptions options, 
            Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            options.Filters.Add(new RestrictDescriptionsToDevelopmentMetricFilter(hostEnvironment));
        }
#else
        /// <summary>
        /// Configure the processors collection to only send headers in the development environment
        /// </summary>
        /// <param name="options"></param>
        /// <param name="hostingEnvironment"></param>
        public static void RestrictMetricsToDevelopment(this ServerTimingOptions options, 
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            options.Filters.Add(new RestrictToDevelopmentMetricFilter(hostingEnvironment));
        }

        /// <summary>
        /// Configure the processors collection to only send headers in the development environment
        /// </summary>
        /// <param name="options"></param>
        /// <param name="hostingEnvironment"></param>
        public static void RemoveDescriptionsOutsideDevelopment(this ServerTimingOptions options, 
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            options.Filters.Add(new RestrictDescriptionsToDevelopmentMetricFilter(hostingEnvironment));
        }
#endif

        /// <summary>
        /// Configure the processors collection to only send headers to a specific IP
        /// </summary>
        /// <param name="options">Options to update</param>
        /// <param name="ip">IP Address to permit</param>
        public static void RestrictMetricsToIp(this ServerTimingOptions options, IPAddress ip)
            => RestrictMetricsToIpRange(options, ip, ip);

        /// <summary>
        /// Configure the processors collection to only send headers to a specific IP
        /// </summary>
        /// <param name="options">Options to update</param>
        /// <param name="ip">IP Address to permit</param>
        public static void RestrictMetricsToIp(this ServerTimingOptions options, string ip)
            => RestrictMetricsToIpRange(options, ip, ip);


        /// <summary>
        /// Configure the processors collection to only send headers to a specific IP range
        /// </summary>
        /// <param name="options">Options to update</param>
        /// <param name="from">Minimum IP Address to permit</param>
        /// <param name="to">Maximum IP Address to permit</param>
        public static void RestrictMetricsToIpRange(this ServerTimingOptions options, string from, string to)
            => options.Filters.Add(new IPRangeMetricFilter(IPAddress.Parse(from), IPAddress.Parse(to)));

        /// <summary>
        /// Configure the processors collection to only send headers to a specific IP range
        /// </summary>
        /// <param name="options">Options to update</param>
        /// <param name="from">Minimum IP Address to permit</param>
        /// <param name="to">Maximum IP Address to permit</param>
        public static void RestrictMetricsToIpRange(this ServerTimingOptions options, IPAddress from, IPAddress to)
            => options.Filters.Add(new IPRangeMetricFilter(from, to));

        /// <summary>
        /// Add a custom processor to filter / modify metrics
        /// </summary>
        /// <param name="options">Options to update</param>
        /// <param name="process">Processing lambda. This may modify the metrics list it is passed, and should return true if
        /// further processors are allowed to run or false if they should be suppressed.</param>
        public static void AddCustomProcessor(this ServerTimingOptions options, Func<HttpContext, ICollection<ServerTimingMetric>, bool> process)
            => options.Filters.Add(new CustomServerTimingMetricFilter(process));

    }
}
