using System.Globalization;
using System.Text.RegularExpressions;

namespace Demo.Azure.Functions.Worker.ServerTiming
{
    public enum ServerTimingDeliveryMode
    {
        Unknown,
        ResponseHeader,
        Trailer
    }

    internal static class HeaderNames
    {
        public const string ServerTiming = "Server-Timing";

        public const string TimingAllowOrigin = "Timing-Allow-Origin";

        internal const string AcceptTransferEncoding = "TE";
    }

    public struct ServerTimingMetric
    {
        private string _serverTimingMetric;

        private static readonly Regex _invalidTokenChars = new Regex("[^&#\\$%&'\\*\\+\\-\\.\\^`\\|~\\w]");

        public string Name { get; }

        public decimal? Value { get; }

        public string Description { get; }

        public ServerTimingMetric(string name)
            : this(name, null, null)
        { }

        public ServerTimingMetric(string name, decimal value)
            : this(name, (decimal?)value, null)
        { }

        public ServerTimingMetric(string name, decimal value, string description)
            : this(name, (decimal?)value, description)
        { }

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
    }

    internal class ServerTimingHeaderValue
    {
        public ICollection<ServerTimingMetric> Metrics { get; }

        public ServerTimingHeaderValue()
        {
            Metrics = new List<ServerTimingMetric>();
        }

        public ServerTimingHeaderValue(ICollection<ServerTimingMetric> metrics)
        {
            Metrics = metrics;
        }

        public ServerTimingHeaderValue(params ServerTimingMetric[] metrics)
            : this()
        {
            foreach (ServerTimingMetric metric in metrics)
            {
                Metrics.Add(metric);
            }
        }

        public override string ToString()
        {
            return String.Join(",", Metrics);
        }
    }

    public interface IServerTiming
    {

        ServerTimingDeliveryMode DeliveryMode { get; }

        ICollection<ServerTimingMetric> Metrics { get; }        
    }

    internal class ServerTiming : IServerTiming
    {
        public ServerTimingDeliveryMode DeliveryMode { get; internal set; } = ServerTimingDeliveryMode.Unknown;

        public ICollection<ServerTimingMetric> Metrics { get; }

        public ServerTiming()
        {
            Metrics = new List<ServerTimingMetric>();
        }
    }
}
