using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Filters;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Test.AspNetCore.ServerTiming.Infrastructure
{
    internal class ServerTimingServerStartup : IStartup
    {
        private const string DELAY_SERVER_TIMING_METRIC_NAME = "DELAY";
        private const int DELAY_SERVER_TIMING_METRIC_VALUE = 100;
        private const string DELAY_SERVER_TIMING_METRIC_DESCRIPTION = "Arbitrary delay";

        private const string RESPONSE_BODY = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Test.AspNetCore.ServerTiming</title></head><body>ServerTimingMiddleware Integration Tests</body></html>";

        private readonly List<IServerTimingMetricFilter> _filters;

        public ServerTimingServerStartup(List<IServerTimingMetricFilter> filters)
        {
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddServerTiming();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseServerTiming(new ServerTimingOptions
            {
                Filters = _filters
            });

            app.Run(async (context) =>
                {
                    IServerTiming serverTiming = context.RequestServices.GetService<IServerTiming>();

                    await Task.Delay(DELAY_SERVER_TIMING_METRIC_VALUE);

                    serverTiming.Metrics.Add(new ServerTimingMetric(DELAY_SERVER_TIMING_METRIC_NAME, DELAY_SERVER_TIMING_METRIC_VALUE, DELAY_SERVER_TIMING_METRIC_DESCRIPTION));

                    await context.Response.WriteAsync(RESPONSE_BODY);
                });
        }
    }
}
