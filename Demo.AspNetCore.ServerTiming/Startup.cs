using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Demo.AspNetCore.ServerTiming
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServerTiming();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServerTiming(options =>
            {
                options.AllowedOrigins.Add("http://localhost");
                options.RestrictMetricsToIp("127.0.0.1");
                options.RestrictMetricsToDevelopment(env);
                
                options.AddCustomProcessor((context, metrics) =>
                {
                    if (System.DateTime.Now.Hour >=18) metrics.Add(new ServerTimingMetric("Time to stop debugging this and go for a drink!"));
                    return true;
                });
            })
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
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
