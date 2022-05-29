using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Filters;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Moq;
using Xunit;

namespace Test.AspNetCore.ServerTiming.Filters
{
    public class RestrictToDevelopmentMetricFilterTests
    {
        #region Prepare SUT
        private RestrictToDevelopmentMetricFilter PrepareRestrictToDevelopmentMetricFilter(bool isDevelopment)
        {
#if !NETCOREAPP2_1 && !NET461
            Mock<Microsoft.Extensions.Hosting.IHostEnvironment> hostEnvironment = new Mock<Microsoft.Extensions.Hosting.IHostEnvironment>();
            hostEnvironment.SetupGet(m => m.EnvironmentName).Returns(isDevelopment ? Microsoft.Extensions.Hosting.Environments.Development : "Test.AspNetCore.ServerTiming");

            return new RestrictToDevelopmentMetricFilter(hostEnvironment.Object);
#else
            Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment> hostEnvironment = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            hostEnvironment.SetupGet(m => m.EnvironmentName).Returns(isDevelopment ? "Development" : "Test.AspNetCore.ServerTiming");

            return new RestrictToDevelopmentMetricFilter(hostEnvironment.Object);
#endif
        }

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
        public void OnServerTimingHeaderPreparation_HostingEnvironmentIsDevelopment_MetricsAreNotCleared()
        {
            RestrictToDevelopmentMetricFilter filter = PrepareRestrictToDevelopmentMetricFilter(true);
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            filter.OnServerTimingHeaderPreparation(new DefaultHttpContext(), metrics);

            Assert.NotEmpty(metrics);
        }

        [Fact]
        public void OnServerTimingHeaderPreparation_HostingEnvironmentIsDevelopment_ReturnsTrue()
        {
            RestrictToDevelopmentMetricFilter filter = PrepareRestrictToDevelopmentMetricFilter(true);
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            bool result = filter.OnServerTimingHeaderPreparation(new DefaultHttpContext(), metrics);

            Assert.True(result);
        }

        [Fact]
        public void OnServerTimingHeaderPreparation_HostingEnvironmentIsNotDevelopment_MetricsAreCleared()
        {
            RestrictToDevelopmentMetricFilter filter = PrepareRestrictToDevelopmentMetricFilter(false);
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            filter.OnServerTimingHeaderPreparation(new DefaultHttpContext(), metrics);

            Assert.Empty(metrics);
        }

        [Fact]
        public void OnServerTimingHeaderPreparation_HostingEnvironmentIsNotDevelopment_ReturnsFalse()
        {
            RestrictToDevelopmentMetricFilter filter = PrepareRestrictToDevelopmentMetricFilter(false);
            ICollection<ServerTimingMetric> metrics = PrepareServerTimingMetrics();

            bool result = filter.OnServerTimingHeaderPreparation(new DefaultHttpContext(), metrics);

            Assert.False(result);
        }
        #endregion
    }
}
