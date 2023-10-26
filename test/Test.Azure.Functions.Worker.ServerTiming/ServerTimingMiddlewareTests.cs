using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Test.Azure.Functions.Worker.ServerTiming.Infrastructure;
using Xunit;

namespace Test.Azure.Functions.Worker.ServerTiming
{
    public class ServerTimingMiddlewareTests : IClassFixture<AzureFunctionsTestcontainersFixture>
    {
        #region Fields
        private const string SERVER_TIMING_HEADER_NAME = "Server-Timing";
        private const string SERVER_TIMING_HEADER_VALUE = "cache;dur=300;desc=\"Cache\",sql;dur=900;desc=\"Sql Server\",fs;dur=600;desc=\"FileSystem\",cpu;dur=1230;desc=\"Total CPU\"";

        private readonly AzureFunctionsTestcontainersFixture _azureFunctionsTestcontainersFixture;
        #endregion

        #region Constructor
        public ServerTimingMiddlewareTests(AzureFunctionsTestcontainersFixture azureFunctionsTestcontainersFixture)
        {
            _azureFunctionsTestcontainersFixture = azureFunctionsTestcontainersFixture;
        }
        #endregion

        #region Tests
        [Fact]
        public async Task Request_ReturnsResponseWithServerTimingHeader()
        {
            HttpClient httpClient = new HttpClient();
            var requestUri = new UriBuilder(Uri.UriSchemeHttp, _azureFunctionsTestcontainersFixture.AzureFunctionsContainerInstance.Hostname, _azureFunctionsTestcontainersFixture.AzureFunctionsContainerInstance.GetMappedPublicPort(80), "api/basic").Uri;

            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            Assert.True(response.Headers.TryGetValues(SERVER_TIMING_HEADER_NAME, out _));
        }

        [Fact]
        public async Task Request_ReturnsResponseWithCorrectServerTimingHeader()
        {
            HttpClient httpClient = new HttpClient();
            var requestUri = new UriBuilder(Uri.UriSchemeHttp, _azureFunctionsTestcontainersFixture.AzureFunctionsContainerInstance.Hostname, _azureFunctionsTestcontainersFixture.AzureFunctionsContainerInstance.GetMappedPublicPort(80), "api/basic").Uri;

            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

            response.Headers.TryGetValues(SERVER_TIMING_HEADER_NAME, out IEnumerable<string> serverTimingHeaderValues);

            Assert.Collection(serverTimingHeaderValues, serverTimingHeaderValue => Assert.Equal(SERVER_TIMING_HEADER_VALUE, serverTimingHeaderValue));
        }
        #endregion
    }
}