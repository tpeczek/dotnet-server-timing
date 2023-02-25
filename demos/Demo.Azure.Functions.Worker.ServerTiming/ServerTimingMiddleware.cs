using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Azure.Functions.Worker.ServerTiming
{
    internal class ServerTimingMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // Pre-function execution

            await next(context);

            // Post-function execution
            InvocationResult invocationResult = context.GetInvocationResult();

            HttpResponseData? response = invocationResult.Value as HttpResponseData;
            if (response is not null)
            {
                IServerTiming serverTiming = context.InstanceServices.GetRequiredService<IServerTiming>();
                response.Headers.Add(HeaderNames.ServerTiming, new ServerTimingHeaderValue(serverTiming.Metrics).ToString());
            }
        }
    }
}
