using System;
using System.Collections.Generic;

namespace Lib.ServerTiming.Http.Headers
{
    /// <summary>
    /// Represents value of Server-Timing header.
    /// </summary>
    public class ServerTimingHeaderValue
    {
        #region Properties
        /// <summary>
        /// Gets the collection of metrics.
        /// </summary>
        public ICollection<ServerTimingMetric> Metrics { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="ServerTimingHeaderValue"/>.
        /// </summary>
        public ServerTimingHeaderValue()
        {
            Metrics = new List<ServerTimingMetric>();
        }

        /// <summary>
        /// Instantiates a new <see cref="ServerTimingHeaderValue"/>.
        /// </summary>
        /// <param name="metrics">The collection of metrics.</param>
        public ServerTimingHeaderValue(ICollection<ServerTimingMetric> metrics)
        {
            Metrics = metrics;
        }

        /// <summary>
        /// Instantiates a new <see cref="ServerTimingHeaderValue"/>.
        /// </summary>
        /// <param name="metrics">The metrics.</param>
        public ServerTimingHeaderValue(params ServerTimingMetric[] metrics)
            : this()
        {
            foreach (ServerTimingMetric metric in metrics)
            {
                Metrics.Add(metric);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the string representation of header value.
        /// </summary>
        /// <returns>The string representation of header value.</returns>
        public override string ToString()
        {
            return String.Join(",", Metrics);
        }
        #endregion
    }
}
