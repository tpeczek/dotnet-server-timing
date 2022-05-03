using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Lib.AspNetCore.ServerTiming.Filters
{
    /// <summary>
    /// Limit when server timing responses are sent based on IP address ranges
    /// </summary>
    public class IpProcessor : IServerTimingMetricFilter
    {
        private readonly byte[] _from;
        private readonly byte[] _to;
        
        /// <summary>
        /// Creates a processor that only permits acess to the given range
        /// </summary>
        /// <param name="from">Lower address in range</param>
        /// <param name="to">Upper address in range</param>
        public IpProcessor(IPAddress from, IPAddress to)
        {
            _from = from.MapToIPv6().GetAddressBytes();
            _to = to.MapToIPv6().GetAddressBytes();
        }

        /// <inheritdoc/>
        public bool OnServerTimingHeaderPreparation(HttpContext context, ICollection<ServerTimingMetric> metrics)
        {
            bool inRange = InRange(context.Connection.RemoteIpAddress.MapToIPv6().GetAddressBytes());

            if (!inRange)
            {
                metrics.Clear();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if the supplied ip address bytes are in the range
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool InRange(byte[] ip) => IsGreaterThanOrEqualTo(ip,_from) || IsLessThanOrEqualTo(ip,_to);

        /// <summary>
        /// Compares two equal length byte arrays and return true if b1 is less than nor equal to b2
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private bool IsGreaterThanOrEqualTo(byte[] b1, byte[] b2)
        {
            int i = 0;
            while (i<b1.Length)
            {                
                int c = b1[i].CompareTo(b2[i]);
                if (c == 1) return true;
                if (c == -1) return false;
                i++; 
            }
            return true;
        }

        /// <summary>
        /// Compares two equal length byte arrays and return true if b1 is less than nor equal to b2
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private bool IsLessThanOrEqualTo(byte[] b1, byte[] b2)
        {
        int i = 0;
        while (i < b1.Length)
        {
            int c = b1[i].CompareTo(b2[i]);
            if (c == 1) return false;
            if (c == -1) return true;
            i++;
        }
        return true;
    }

}
}
