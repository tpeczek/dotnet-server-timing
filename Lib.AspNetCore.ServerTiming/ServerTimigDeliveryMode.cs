namespace Lib.AspNetCore.ServerTiming
{
    /// <summary>
    /// The metrics delivery mode.
    /// </summary>
    public enum ServerTimigDeliveryMode
    {
        /// The metrics delivery mode is no yet known.
        Unknown,
        /// The metrics will be delivered through request header.
        ResponseHeader,
        /// The metrics will be delivered through trailer.
        Trailer
    }
}
