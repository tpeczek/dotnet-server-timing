using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.Extensions.Hosting;

namespace Demo.AspNetCore.ServerTiming
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServerTiming();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServerTiming("http://localhost:8000", "http://localhost:8001")
                .Run(async (context) =>
                {
                    IServerTiming serverTiming = context.RequestServices.GetRequiredService<IServerTiming>();
                    serverTiming.Metrics.Add(new ServerTimingMetric("cache", 300, "Cache"));
                    serverTiming.Metrics.Add(new ServerTimingMetric("sql", 900, "Sql Server"));
                    serverTiming.Metrics.Add(new ServerTimingMetric("fs", 600, "FileSystem"));
                    serverTiming.Metrics.Add(new ServerTimingMetric("cpu", 1230, "Total CPU"));

                    await context.Response.WriteAsync("-- Demo.AspNetCore.ServerTiming --");
                });
        }
    }
}
