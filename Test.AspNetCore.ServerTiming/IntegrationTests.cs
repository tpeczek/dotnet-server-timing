using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Test.AspNetCore.ServerTiming
{
	[Collection(nameof(TestServerCollection))]
	public class IntegrationTests
	{
		private readonly TestServerFixture<Startup> _fixture;
		private readonly HttpClient _client;

		public IntegrationTests(TestServerFixture<Startup> fixture)
		{
			_fixture = fixture;
			_client = fixture.Client;
		}

		[Fact]
		public async Task SimplePageIndex()
		{
			// Arrange
			// Act
			using (HttpResponseMessage response = await _client.GetAsync("/"))
			{
				// Assert
				response.EnsureSuccessStatusCode();

				var responseString = await response.Content.ReadAsStringAsync();

				Assert.NotNull(responseString);
				Assert.Contains("Title of the document", responseString);
			}
		}

		[Theory]
		[InlineData("test", 100, "test desc")]
		public async Task ServerTimingResponseHeader(string name, decimal value, string description)
		{
			// Arrange
			// Act
			using (HttpResponseMessage response = await _client.GetAsync("/"))
			{
				// Assert
				response.EnsureSuccessStatusCode();

				var responseString = await response.Content.ReadAsStringAsync();

				Assert.NotNull(response.Headers);

				/**
				 * 
				 * Server-Timing: test;dur=100;desc="test entry"
				 * 
				 * */
				Assert.True(response.Headers.TryGetValues(Lib.AspNetCore.ServerTiming.Http.Headers.HeaderNames.ServerTiming, out IEnumerable<string> serv_headers_values));
				Assert.NotEmpty(serv_headers_values);

				var tab = serv_headers_values.First().Split(";".ToArray(), 3, StringSplitOptions.RemoveEmptyEntries);

				Assert.NotNull(tab);
				Assert.True(tab.Length == 3);
				Assert.Equal(tab[0].Trim(), name);
				Assert.Equal(tab[1].Trim(), $"dur={value}");
				Assert.Equal(tab[2].Trim(), $"desc=\"{description}\"");
			}
		}
	}
}
