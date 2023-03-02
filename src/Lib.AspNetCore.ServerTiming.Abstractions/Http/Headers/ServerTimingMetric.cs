﻿using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Lib.AspNetCore.ServerTiming.Http.Headers
{
    /// <summary>
    /// Server timing performance metric.
    /// </summary>
    public struct ServerTimingMetric
    {
        #region Fields
        private string _serverTimingMetric;
        
        // Regex to find chars that are invalid in https://httpwg.org/specs/rfc7230.html#rfc.section.3.2.6
        private static readonly Regex _invalidTokenChars = new Regex("[^&#\\$%&'\\*\\+\\-\\.\\^`\\|~\\w]");
        #endregion

        #region Properties
        /// <summary>
        /// Gets the metric name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the metric value.
        /// </summary>
        public decimal? Value { get; }

        /// <summary>
        /// Gets the metric description.
        /// </summary>
        public string Description { get; }
        #endregion

        #region Constructors
        /// <summary>
        ///  Initializes a new <see cref="ServerTimingMetric"/>.
        /// </summary>
        /// <param name="name">The metric name.</param>
        public ServerTimingMetric(string name)
            : this(name, null, null)
        { }

        /// <summary>
        ///  Initializes a new <see cref="ServerTimingMetric"/>.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        public ServerTimingMetric(string name, decimal value)
            : this(name, (decimal?)value, null)
        { }

        /// <summary>
        ///  Initializes a new <see cref="ServerTimingMetric"/>.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="value">The metric value.</param>
        /// <param name="description">The metric description.</param>
        public ServerTimingMetric(string name, decimal value, string description)
            : this(name, (decimal?)value, description)
        { }

        /// <summary>
        ///  Initializes a new <see cref="ServerTimingMetric"/>.
        /// </summary>
        /// <param name="name">The metric name.</param>
        /// <param name="description">The metric description.</param>
        public ServerTimingMetric(string name, string description)
            : this(name, null, description)
        { }

        internal ServerTimingMetric(string name, decimal? value, string description)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Value = value;
            Description = description;

            _serverTimingMetric = null;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the string representation of metric.
        /// </summary>
        /// <returns>The string representation of metric.</returns>
        public override string ToString()
        {
            if (_serverTimingMetric is null)
            {
                _serverTimingMetric = _invalidTokenChars.Replace(Name.Replace(' ', '-'), "");

                if (Value.HasValue)
                {
                    _serverTimingMetric = _serverTimingMetric + ";dur=" + Value.Value.ToString(CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrEmpty(Description))
                {
                    _serverTimingMetric = _serverTimingMetric + ";desc=\"" + Description + "\"";
                }
            }

            return _serverTimingMetric;
        }
        #endregion
    }
}
