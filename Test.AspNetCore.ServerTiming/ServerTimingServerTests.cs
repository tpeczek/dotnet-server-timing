using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Lib.AspNetCore.ServerTiming.Filters;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Moq;
using Xunit;
using Test.AspNetCore.ServerTiming.Infrastructure;

namespace Test.AspNetCore.ServerTiming
{
    public class ServerTimingServerTests
    {
        #region Fields
        private const string SERVER_TIMING_HEADER_NAME = "Server-Timing";
        private const string SERVER_TIMING_HEADER_VALUE = "DELAY;dur=100;desc=\"Arbitrary delay\"";
        #endregion

        #region Prepare SUT
        private TestServer PrepareTestServer(List<IServerTimingMetricFilter> filters = null)
        {
            IWebHostBuilder webHostBuilder = new WebHostBuilder().ConfigureServices(services =>
            {
                services.AddSingleton<IStartup>(new ServerTimingServerStartup(filters ?? new List<IServerTimingMetricFilter>()));
            });

            return new TestServer(webHostBuilder);
        }

        private Mock<IServerTimingMetricFilter> PrepareServerTimingMetricFilterMock(bool onServerTimingHeaderPreparationResult = true)
        {
            Mock<IServerTimingMetricFilter> serverTimingMetricFilterMock = new Mock<IServerTimingMetricFilter>();

            serverTimingMetricFilterMock.Setup(m => m.OnServerTimingHeaderPreparation(It.IsAny<HttpContext>(), It.IsAny<ICollection<ServerTimingMetric>>())).Returns(onServerTimingHeaderPreparationResult);

            return serverTimingMetricFilterMock;
        }
        #endregion

        #region Tests
        [Fact]
        public async Task Request_ReturnsResponseWithServerTimingHeader()
        {
            using (TestServer server = PrepareTestServer())
            {
                using (HttpClient client = server.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync("/");
                    
                    Assert.True(response.Headers.TryGetValues(SERVER_TIMING_HEADER_NAME, out IEnumerable<string> serverTimingHeaderValue));
                }
            }
        }

        [Fact]
        public async Task Request_ReturnsResponseWithCorrectServerTimingHeader()
        {
            using (TestServer server = PrepareTestServer())
            {
                using (HttpClient client = server.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync("/");

                    response.Headers.TryGetValues(SERVER_TIMING_HEADER_NAME, out IEnumerable<string> serverTimingHeaderValues);

                    Assert.Collection(serverTimingHeaderValues, serverTimingHeaderValue => Assert.Equal(SERVER_TIMING_HEADER_VALUE, serverTimingHeaderValue));
                }
            }
        }

#if !NETCOREAPP2_1 && !NET461
        [Fact]
        public async Task Request_AllowsTrailers_ReturnsResponseWithServerTimingTrailer()
        {
            using (TestServer server = PrepareTestServer())
            {
                using (HttpClient client = server.CreateClient())
                {
                    client.DefaultRequestVersion = System.Net.HttpVersion.Version20;
                    client.DefaultRequestHeaders.Add("TE", "trailers");

                    HttpResponseMessage response = await client.GetAsync("/");

                    Assert.True(response.TrailingHeaders.TryGetValues(SERVER_TIMING_HEADER_NAME, out IEnumerable<string> serverTimingHeaderValue));
                }
            }
        }
#endif

        [Fact]
        public async Task Request_TwoFiltersRegistered_FiltersNotShortCircuiting_AllFiltersRun()
        {
            Mock<IServerTimingMetricFilter> firstServerTimingMetricFilter = PrepareServerTimingMetricFilterMock();
            Mock<IServerTimingMetricFilter> secondServerTimingMetricFilter = PrepareServerTimingMetricFilterMock();

            using (TestServer server = PrepareTestServer(new List<IServerTimingMetricFilter>
            {
                firstServerTimingMetricFilter.Object,
                secondServerTimingMetricFilter.Object
            }))
            {
                using (HttpClient client = server.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync("/");

                    firstServerTimingMetricFilter.Verify(m => m.OnServerTimingHeaderPreparation(It.IsAny<HttpContext>(), It.IsAny<ICollection<ServerTimingMetric>>()), Times.Once);
                    secondServerTimingMetricFilter.Verify(m => m.OnServerTimingHeaderPreparation(It.IsAny<HttpContext>(), It.IsAny<ICollection<ServerTimingMetric>>()), Times.Once);
                }
            }
        }

        [Fact]
        public async Task Request_TwoFiltersRegistered_FirstFilterShortCircuiting_OnlyFirstFiltersRun()
        {
            Mock<IServerTimingMetricFilter> firstServerTimingMetricFilter = PrepareServerTimingMetricFilterMock(false);
            Mock<IServerTimingMetricFilter> secondServerTimingMetricFilter = PrepareServerTimingMetricFilterMock();

            using (TestServer server = PrepareTestServer(new List<IServerTimingMetricFilter>
            {
                firstServerTimingMetricFilter.Object,
                secondServerTimingMetricFilter.Object
            }))
            {
                using (HttpClient client = server.CreateClient())
                {
                    HttpResponseMessage response = await client.GetAsync("/");

                    firstServerTimingMetricFilter.Verify(m => m.OnServerTimingHeaderPreparation(It.IsAny<HttpContext>(), It.IsAny<ICollection<ServerTimingMetric>>()), Times.Once);
                    secondServerTimingMetricFilter.Verify(m => m.OnServerTimingHeaderPreparation(It.IsAny<HttpContext>(), It.IsAny<ICollection<ServerTimingMetric>>()), Times.Never);
                }
            }
        }
        #endregion
    }
}
