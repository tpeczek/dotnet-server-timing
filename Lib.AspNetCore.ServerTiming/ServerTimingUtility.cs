namespace Lib.AspNetCore.ServerTiming 
{
    using Lib.AspNetCore.ServerTiming.Http.Headers;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>Utilities for easier logging of timings.</summary>
    static class ServerTimingUtility
    {
        /// <summary>Time the content of a using block.</summary>
        /// <param name="timing">The logger to write to.</param>
        /// <param name="metricName">Optional, metric name, if passed will override any called name.</param>
        /// <param name="functionName">Optional, populated compile-time with the name of the calling function</param>
        /// <param name="filePath">Optional, populated compile-time with the path to the calling file</param>
        /// <param name="lineNumber">Optional, populated compile-time with line number in the calling file</param>
        public static IDisposable TimeAction(this IServerTiming timing,
            string metricName = null,
            [CallerMemberName] string functionName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (timing == null)
                return null;

            string caller = metricName ?? FormatCallerName(functionName, filePath, lineNumber);
            return new ServerTimingInstance(timing, caller);
        }

        /// <summary>Time an async task.</summary>
        /// <typeparam name="T">Type of the tasl</typeparam>
        /// <param name="task">The task to time</param>
        /// <param name="timing">The logger to write to.</param>
        /// <param name="metricName">Optional, metric name, if passed will override any called name.</param>
        /// <param name="functionName">Optional, populated compile-time with the name of the calling function</param>
        /// <param name="filePath">Optional, populated compile-time with the path to the calling file</param>
        /// <param name="lineNumber">Optional, populated compile-time with line number in the calling file</param>
        /// <returns></returns>
        public static async Task<T> TimeTask<T>(this IServerTiming timing,
            Task<T> task,
            string metricName = null,
            [CallerMemberName] string functionName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            using (timing.TimeAction(metricName, functionName, filePath, lineNumber))
                return await task;
        }

        /// <summary>Add a metric to the timing, if present.</summary>
        /// <param name="timing">The timings to extend</param>
        /// <param name="duration">The duration to log in ms.</param>
        /// <param name="metricName">The name of the metric to log.</param>
        /// <param name="functionName">Optional, populated compile-time with the name of the calling function</param>
        /// <param name="filePath">Optional, populated compile-time with the path to the calling file</param>
        /// <param name="lineNumber">Optional, populated compile-time with line number in the calling file</param>
        public static void AddMetric(this IServerTiming timing, decimal duration,
            string metricName = null,
            [CallerMemberName] string functionName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (timing == null)
                return;

            var metric = new ServerTimingMetric(metricName ?? FormatCallerName(functionName, filePath, lineNumber), duration);
            timing.Metrics.Add(metric);
        }

        /// <summary>Format a metric name from caller params.</summary>
        /// <returns>Generated metric name of "{fileName}.{function}+{lineNumber}</returns>
        static string FormatCallerName(string functionName, string filePath, int lineNumber) =>
            string.Concat(Path.GetFileNameWithoutExtension(filePath), ".", functionName, "+", lineNumber);

        /// <summary>On dispose log the duration to the timing.</summary>
        sealed class ServerTimingInstance : IDisposable
        {
            readonly IServerTiming timing;
            readonly string metricName;
            readonly Stopwatch watch;

            public ServerTimingInstance(IServerTiming timing, string metricName)
            {
                this.timing = timing;
                this.metricName = metricName;
                this.watch = new Stopwatch();
                this.watch.Start();
            }

            void IDisposable.Dispose()
            {
                this.watch.Stop();
                this.timing.AddMetric(this.watch.ElapsedMilliseconds, this.metricName);
            }
        }
    }
}
