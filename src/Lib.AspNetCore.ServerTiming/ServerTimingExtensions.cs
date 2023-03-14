using Lib.ServerTiming;

namespace Lib.AspNetCore.ServerTiming 
{
    internal static class ServerTimingExtensions
    {
        internal static void SetServerTimingDeliveryMode(this IServerTiming serverTiming, ServerTimingDeliveryMode deliveryMode)
        {
            ServerTiming concreteServerTiming = serverTiming as ServerTiming;

            if (concreteServerTiming != null)
            {
                concreteServerTiming.DeliveryMode = deliveryMode;
            }
        }
    }
}
