using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Lib.Azure.Functions.Worker.ServerTiming
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerTimingMiddleware : IFunctionsWorkerMiddleware
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            await next(context);
        }
    }
}
