using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.ServerTiming.Filters;
using Lib.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Filters
{
    /// <summary>
    /// A filter which will remove all metrics unless the current request IP address falls within the specified range.
    /// </summary>
    public class IPRangeMetricFilter : IServerTimingMetricFilter<HttpContext>
    {
        private readonly AddressFamily _addressFamily;
        private readonly byte[] _lowerInclusiveBytes;
        private readonly byte[] _upperInclusiveBytes;

        /// <summary>
        /// Instantiates a new <see cref="IPRangeMetricFilter"/>.
        /// </summary>
        /// <param name="lowerInclusive">Lower (inclusive) bound of the IP addresses range.</param>
        /// <param name="upperInclusive">Upper (inclusive) bound of the IP addresses range.</param>
        public IPRangeMetricFilter(IPAddress lowerInclusive, IPAddress upperInclusive)
        {
            _addressFamily = lowerInclusive.MapToIPv6().AddressFamily;
            _lowerInclusiveBytes = lowerInclusive.MapToIPv6().GetAddressBytes();
            _upperInclusiveBytes = upperInclusive.MapToIPv6().GetAddressBytes();
        }

        /// <summary>
        /// Removes all metrics for current request unless the current request IP address falls within the specified range.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="metrics">The collection of metrics for current request.</param>
        /// <returns>True if subsequent filters are allowed to run, otherwise false.</returns>
        public bool OnServerTimingHeaderPreparation(HttpContext context, ICollection<ServerTimingMetric> metrics)
        {
            bool inRange = InRange(context.Connection.RemoteIpAddress);

            if (!inRange)
            {
                metrics.Clear();

                return false;
            }

            return true;
        }

        private bool InRange(IPAddress address)
        {
            IPAddress addressToIPv6 = address.MapToIPv6();
            if (addressToIPv6.AddressFamily != _addressFamily)
            {
                return false;
            }

            byte[] addressBytes = addressToIPv6.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < _lowerInclusiveBytes.Length && (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < _lowerInclusiveBytes[i]) || (upperBoundary && addressBytes[i] > _upperInclusiveBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == _lowerInclusiveBytes[i]);
                upperBoundary &= (addressBytes[i] == _upperInclusiveBytes[i]);
            }

            return true;
        }
    }
}
