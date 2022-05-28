using System.Collections.Generic;
using Lib.AspNetCore.ServerTiming.Abstractions;
using Lib.AspNetCore.ServerTiming.Abstractions.Http.Headers;

namespace Lib.AspNetCore.ServerTiming
{
    internal class ServerTiming : IServerTiming
    {
        #region Properties
        public ServerTimingDeliveryMode DeliveryMode { get; internal set; } = ServerTimingDeliveryMode.Unknown;

        public ICollection<ServerTimingMetric> Metrics { get; }
        #endregion

        #region Constructors
        public ServerTiming()
        {
            Metrics = new List<ServerTimingMetric>();
        }
        #endregion
    }
}
