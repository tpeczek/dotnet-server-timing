using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming 
{
    /// <summary>
    /// Utilities for easier logging of server timing performance metric.
    /// </summary>
    public static class ServerTimingUtility
    {
        /// <summary>
        /// Time a block of code.
        /// </summary>
        /// <param name="serverTiming">The <see cref="IServerTiming"/> to add metric to.</param>
        /// <param name="metricName">Optional, metric name, if passed will override any caller name.</param>
        /// <param name="functionName">Optional, populated compile-time with the name of the calling function.</param>
        /// <param name="filePath">Optional, populated compile-time with the path to the calling file.</param>
        /// <param name="lineNumber">Optional, populated compile-time with line number in the calling file.</param>
        public static IDisposable TimeAction(this IServerTiming serverTiming,
            string metricName = null,
            [CallerMemberName] string functionName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (serverTiming is null) 
            {
                return null;
            }

            string caller = metricName ?? FormatCallerName(functionName, filePath, lineNumber);

            return new ServerTimingInstance(serverTiming, caller);
        }

        /// <summary>
        /// Time an async task.
        /// </summary>
        /// <typeparam name="T">Type of the task.</typeparam>
        /// <param name="task">The <see cref="Task"/> to time.</param>
        /// <param name="serverTiming">The <see cref="IServerTiming"/> to add metric to.</param>
        /// <param name="metricName">Optional, metric name, if passed will override any caller name.</param>
        /// <param name="functionName">Optional, populated compile-time with the name of the calling function.</param>
        /// <param name="filePath">Optional, populated compile-time with the path to the calling file.</param>
        /// <param name="lineNumber">Optional, populated compile-time with line number in the calling file.</param>
        /// <returns></returns>
        public static async Task<T> TimeTask<T>(this IServerTiming serverTiming,
            Task<T> task,
            string metricName = null,
            [CallerMemberName] string functionName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            using (serverTiming.TimeAction(metricName, functionName, filePath, lineNumber)) 
            {
                return await task;
            }
        }

        /// <summary>
        /// Add a metric to the timing, if present.
        /// </summary>
        /// <param name="serverTiming">The <see cref="IServerTiming"/> to add metric to.</param>
        /// <param name="duration">The duration to log in ms.</param>
        /// <param name="metricName">The name of the metric to log.</param>
        /// <param name="functionName">Optional, populated compile-time with the name of the calling function</param>
        /// <param name="filePath">Optional, populated compile-time with the path to the calling file</param>
        /// <param name="lineNumber">Optional, populated compile-time with line number in the calling file</param>
        public static void AddMetric(this IServerTiming serverTiming, decimal duration,
            string metricName = null,
            [CallerMemberName] string functionName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (serverTiming is null)
            {
                return;
            }

            ServerTimingMetric metric = new ServerTimingMetric(metricName ?? FormatCallerName(functionName, filePath, lineNumber), duration);

            serverTiming.Metrics.Add(metric);
        }

        // Format a metric name from caller params --> "{fileName}.{function}+{lineNumber}
        private static string FormatCallerName(string functionName, string filePath, int lineNumber) =>
            String.Concat(Path.GetFileNameWithoutExtension(filePath), ".", functionName, "+", lineNumber);

        // On dispose, add server timing performance metric.
        private sealed class ServerTimingInstance : IDisposable
        {
            private readonly IServerTiming _serverTiming;
            private readonly string _metricName;
            private readonly Stopwatch _watch;

            public ServerTimingInstance(IServerTiming serverTiming, string metricName)
            {
                _serverTiming = serverTiming;
                _metricName = metricName;

                _watch = new Stopwatch();
                _watch.Start();
            }

            public void Dispose()
            {
                _watch.Stop();

                _serverTiming.AddMetric(_watch.ElapsedMilliseconds, _metricName);
            }
        }
    }
}
