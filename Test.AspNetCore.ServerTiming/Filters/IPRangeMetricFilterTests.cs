using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Filters;
using Lib.AspNetCore.ServerTiming.Abstractions.Http.Headers;
using Xunit;


namespace Test.AspNetCore.ServerTiming.Filters
{
    public class IPRangeMetricFilterTests
    {
        #region Prepare SUT
        private ICollection<ServerTimingMetric> PrepareServerTimingMetrics()
        {
            return new List<ServerTimingMetric>
            {
                new ServerTimingMetric("cache", 300, "Cache"),
                new ServerTimingMetric("sql", 900, "Sql Server"),
                new ServerTimingMetric("fs", 600, "FileSystem"),
                new ServerTimingMetric("cpu", 1230, "Total CPU")
            };
        }
        #endregion

        #region Tests
        [Fact]
        public void OnServerTimingHeaderPreparation_IPAddressWithinRange_MetricsAreNotCleared()
        {
            IPRangeMetricFilter filter = new IPRangeMetricFilter(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.0.255"));
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            HttpContext httpContext = new DefaultHttpContext
            {
                Connection =
                {
                    RemoteIpAddress = IPAddress.Parse("192.168.0.34")
                }
            };

            filter.OnServerTimingHeaderPreparation(httpContext, metrics);

            Assert.NotEmpty(metrics);
        }

        [Fact]
        public void OnServerTimingHeaderPreparation_IPAddressWithinRange_ReturnsTrue()
        {
            IPRangeMetricFilter filter = new IPRangeMetricFilter(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.0.255"));
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            HttpContext httpContext = new DefaultHttpContext
            {
                Connection =
                {
                    RemoteIpAddress = IPAddress.Parse("192.168.0.34")
                }
            };

            bool result = filter.OnServerTimingHeaderPreparation(httpContext, metrics);

            Assert.True(result);
        }

        [Fact]
        public void OnServerTimingHeaderPreparation_IPAddressOutsideRange_MetricsAreCleared()
        {
            IPRangeMetricFilter filter = new IPRangeMetricFilter(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.0.255"));
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            HttpContext httpContext = new DefaultHttpContext
            {
                Connection =
                {
                    RemoteIpAddress = IPAddress.Parse("192.168.10.1")
                }
            };

            filter.OnServerTimingHeaderPreparation(httpContext, metrics);

            Assert.Empty(metrics);
        }

        [Fact]
        public void OnServerTimingHeaderPreparation_IPAddressOutsideRange_ReturnsFalse()
        {
            IPRangeMetricFilter filter = new IPRangeMetricFilter(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.0.255"));
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            HttpContext httpContext = new DefaultHttpContext
            {
                Connection =
                {
                    RemoteIpAddress = IPAddress.Parse("192.168.10.1")
                }
            };

            bool result = filter.OnServerTimingHeaderPreparation(httpContext, metrics);

            Assert.False(result);
        }
        #endregion
    }
}
