using Lib.AspNetCore.ServerTiming;
using Lib.AspNetCore.ServerTiming.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Test.AspNetCore.ServerTiming
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddServerTiming();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseServerTiming();

			app.Run(async (context) =>
			{
				var serverTiming = context.RequestServices.GetService<IServerTiming>();

				string page = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Title of the document</title></head>" +
					"<body>Content of the document......</body></html>";

				const int delay = 100;
				await Task.Delay(delay);
				serverTiming.Metrics.Add(new ServerTimingMetric("test", (decimal)delay, "test desc"));

				await context.Response.WriteAsync(page);
			});
		}
	}
}
