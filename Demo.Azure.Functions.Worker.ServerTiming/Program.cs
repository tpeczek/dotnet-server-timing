using Microsoft.Extensions.Hosting;
using Demo.Azure.Functions.Worker.ServerTiming;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        // Register middleware with the worker
        workerApplication.UseMiddleware<ServerTimingMiddleware>();
    })
    .ConfigureServices(s =>
    {
        s.AddScoped<IServerTiming, ServerTiming>();
    })
    .Build();

host.Run();
