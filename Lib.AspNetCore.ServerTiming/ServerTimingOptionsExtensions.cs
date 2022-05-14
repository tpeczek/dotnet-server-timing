using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Filters;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// The <see cref="ServerTimingOptions"/> extensions for registering <see cref="IServerTimingMetricFilter"/>s.
    /// </summary>
    public static class ServerTimingOptionsExtensions
    {
#if !NETCOREAPP2_1 && !NET461
        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove all metrics unless an application is running in development environment.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="hostEnvironment">The <see cref="Microsoft.Extensions.Hosting.IHostEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public static void RestrictMetricsToDevelopment(this ServerTimingOptions options, Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            options.Filters.Add(new RestrictToDevelopmentMetricFilter(hostEnvironment));
        }

        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove the descriptions from all metrics unless an application is running in development environment.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="hostEnvironment">The <see cref="Microsoft.Extensions.Hosting.IHostEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public static void RestrictDescriptionsToDevelopment(this ServerTimingOptions options, Microsoft.Extensions.Hosting.IHostEnvironment hostEnvironment)
        {
            options.Filters.Add(new RestrictDescriptionsToDevelopmentMetricFilter(hostEnvironment));
        }
#else
        /// <summary>.
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove all metrics unless an application is running in development environment.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="hostingEnvironment">The <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> used to determine the hosting environment an application is running in.</param>
        public static void RestrictMetricsToDevelopment(this ServerTimingOptions options, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            options.Filters.Add(new RestrictToDevelopmentMetricFilter(hostingEnvironment));
        }

        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove the descriptions from all metrics unless an application is running in development environment.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="hostingEnvironment">The <see cref="Microsoft.AspNetCore.Hosting.IHostingEnvironment"/> used to determine the hosting environment an application is running in</param>
        public static void RemoveDescriptionsOutsideDevelopment(this ServerTimingOptions options, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            options.Filters.Add(new RestrictDescriptionsToDevelopmentMetricFilter(hostingEnvironment));
        }
#endif

        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove all metrics unless request comes from specific IP address.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="address">The IP Address which is allowed to receive the metrics.</param>
        public static void RestrictMetricsToIp(this ServerTimingOptions options, IPAddress address) => RestrictMetricsToIpRange(options, address, address);

        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove all metrics unless request comes from specific IP address.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="address">The IP Address which is allowed to receive the metrics.</param>
        public static void RestrictMetricsToIp(this ServerTimingOptions options, string address) => RestrictMetricsToIpRange(options, address, address);

        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove all metrics unless request comes from IP address which falls within the specified range.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="lowerInclusive">The lower (inclusive) bound of the IP addresses range.</param>
        /// <param name="upperInclusive">The upper (inclusive) bound of the IP addresses range.</param>
        public static void RestrictMetricsToIpRange(this ServerTimingOptions options, string lowerInclusive, string upperInclusive) => options.Filters.Add(new IPRangeMetricFilter(IPAddress.Parse(lowerInclusive), IPAddress.Parse(upperInclusive)));

        /// <summary>
        /// Adds <see cref="IServerTimingMetricFilter"/> which will remove all metrics unless request comes from IP address which falls within the specified range.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="lowerInclusive">The lower (inclusive) bound of the IP addresses range.</param>
        /// <param name="upperInclusive">The upper (inclusive) bound of the IP addresses range.</param>
        public static void RestrictMetricsToIpRange(this ServerTimingOptions options, IPAddress lowerInclusive, IPAddress upperInclusive) => options.Filters.Add(new IPRangeMetricFilter(lowerInclusive, upperInclusive));

        /// <summary>
        /// Adds a custom <see cref="IServerTimingMetricFilter"/>.
        /// </summary>
        /// <param name="options">The <see cref="ServerTimingOptions"/> to modify.</param>
        /// <param name="filter">The function used for inspecting and modifying the collection of metrics.</param>
        public static void AddCustomMetricFilter(this ServerTimingOptions options, Func<HttpContext, ICollection<ServerTimingMetric>, bool> filter) => options.Filters.Add(new CustomServerTimingMetricFilter(filter));

    }
}
