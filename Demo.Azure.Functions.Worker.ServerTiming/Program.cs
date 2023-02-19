using Microsoft.Extensions.Hosting;
using Demo.Azure.Functions.Worker.ServerTiming;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        // Register middleware with the worker
        workerApplication.UseMiddleware<ServerTimingMiddleware>();
    })
    .Build();

host.Run();
