using System;
using System.Collections.Generic;

namespace Lib.ServerTiming.Http.Headers
{
    /// <summary>
    /// Represents value of Timing-Allow-Origin header.
    /// </summary>
    public class TimingAllowOriginHeaderValue
    {
        #region Properties
        /// <summary>
        /// Gets the collection of origins that are allowed to see values from timing APIs.
        /// </summary>
        public ICollection<string> Origins { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiates a new <see cref="TimingAllowOriginHeaderValue"/>.
        /// </summary>
        public TimingAllowOriginHeaderValue()
        {
            Origins = new List<string>();
        }

        /// <summary>
        /// Instantiates a new <see cref="TimingAllowOriginHeaderValue"/>.
        /// </summary>
        /// <param name="origins">The collection of origins that are allowed to see values from timing APIs.</param>
        public TimingAllowOriginHeaderValue(ICollection<string> origins)
        {
            Origins = origins;
        }

        /// <summary>
        /// Instantiates a new <see cref="TimingAllowOriginHeaderValue"/>.
        /// </summary>
        /// <param name="origins">The origins that are allowed to see values from timing APIs.</param>
        public TimingAllowOriginHeaderValue(params string[] origins)
            : this()
        {
            foreach (string origin in origins)
            {
                Origins.Add(origin);
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
            return String.Join(",", Origins);
        }
        #endregion
    }
}
