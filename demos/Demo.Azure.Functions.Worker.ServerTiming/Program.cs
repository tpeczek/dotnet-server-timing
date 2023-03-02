using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Lib.Azure.Functions.Worker.ServerTiming;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<ServerTimingMiddleware>();
    })
    .ConfigureServices(s =>
    {
        s.AddServerTiming();
    })
    .Build();

host.Run();
