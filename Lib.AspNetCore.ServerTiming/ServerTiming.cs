using System.Collections.Generic;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming
{
    internal class ServerTiming : IServerTiming
    {
        #region Properties
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
