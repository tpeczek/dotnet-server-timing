namespace Lib.ServerTiming
{
    /// <summary>
    /// The metrics delivery mode.
    /// </summary>
    public enum ServerTimingDeliveryMode
    {
        /// The metrics delivery mode is no yet known.
        Unknown,
        /// The metrics will be delivered through request header.
        ResponseHeader,
        /// The metrics will be delivered through trailer.
        Trailer
    }
}
