using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Test.AspNetCore.ServerTiming.Infrastructure
{
    internal class ServerTimingServerStartup
    {
        private const string DELAY_SERVER_TIMING_METRIC_NAME = "DELAY";
        private const int DELAY_SERVER_TIMING_METRIC_VALUE = 100;
        private const string DELAY_SERVER_TIMING_METRIC_DESCRIPTION = "Arbitrary delay";

        private const string RESPONSE_BODY = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Test.AspNetCore.ServerTiming</title></head><body>ServerTimingMiddleware Integration Tests</body></html>";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServerTiming();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseServerTiming()
                .Run(async (context) =>
                {
                    IServerTiming serverTiming = context.RequestServices.GetService<IServerTiming>();

                    await Task.Delay(DELAY_SERVER_TIMING_METRIC_VALUE);

                    serverTiming.Metrics.Add(new ServerTimingMetric(DELAY_SERVER_TIMING_METRIC_NAME, DELAY_SERVER_TIMING_METRIC_VALUE, DELAY_SERVER_TIMING_METRIC_DESCRIPTION));

                    await context.Response.WriteAsync(RESPONSE_BODY);
                });
        }
    }
}
