using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        //workerApplication.UseServerTiming(new()
        //{
        //    AllowedOrigins = new List<string> { "https://tpeczek.com", "https://developer.tpeczek.com" }
        //});
        workerApplication.UseServerTiming();
    })
    .ConfigureServices(s =>
    {
        s.AddServerTiming();
    })
    .Build();

host.Run();
