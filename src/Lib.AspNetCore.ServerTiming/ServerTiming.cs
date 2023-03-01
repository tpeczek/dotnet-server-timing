using System.Collections.Generic;
using Lib.ServerTiming;
using Lib.ServerTiming.Http.Headers;

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
