using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
        private TestServer PrepareTestServer()
        {
            IWebHostBuilder webHostBuilder = new WebHostBuilder()
                .UseStartup<ServerTimingServerStartup>();

            return new TestServer(webHostBuilder);
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
        #endregion
    }
}
