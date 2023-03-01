using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lib.ServerTiming;
using Lib.ServerTiming.Http.Headers;

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

            app.UseServerTiming()
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
