using System.Collections.Generic;
using Lib.ServerTiming;
using Lib.ServerTiming.Http.Headers;

namespace Lib.Azure.Functions.Worker.ServerTiming
{
    internal class ServerTiming : IServerTiming
    {
        #region Properties
        public ServerTimingDeliveryMode DeliveryMode { get; } = ServerTimingDeliveryMode.ResponseHeader;

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
