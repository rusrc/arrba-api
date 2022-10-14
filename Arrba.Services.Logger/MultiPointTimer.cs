using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Arrba.Services.Logger
{
    public class MultiPointTimer : IDisposable
    {
        private readonly Stopwatch _stopWatch;
        private readonly string _message;
        private readonly ILogService _logger;
        private readonly LogLevel _level;
        private readonly List<(string Name, TimeSpan Elapsed)> _points;
        private TimeSpan _last;

        /// <summary>
        /// Constructor initializing the instance and starting its internal timer
        /// </summary>
        /// <param name="message">Leading message text. Will get appended by the total time and the list of component times</param>
        /// <param name="logger">Logger instance to log messages to</param>
        /// <param name="logLevel">Log level to log the message at</param>
        public MultiPointTimer(string message, ILogService logger, LogLevel logLevel = LogLevel.Info)
        {
            _stopWatch = new Stopwatch();
            _message = message ?? throw new ArgumentNullException(nameof(message));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _level = logLevel;
            _points = new List<(string Name, TimeSpan Elapsed)>();
            _last = new TimeSpan(0);
            _stopWatch.Start();
        }

        /// <summary>
        /// Add a time point. The time is relative to the previous call to AddPoint 
        /// or the construction of the instance if this is the first call
        /// </summary>
        /// <param name="name">Name to give the point</param>
        public void AddPoint(string name)
        {
            var now = _stopWatch.Elapsed;
            _points.Add((name, now - _last));
            _last = now;
        }

        /// <summary>
        /// Add a time point. The timing is provided by the caller.
        /// Useful for cases with multiple parallel operation timings.
        /// </summary>
        /// <param name="name">Name to give the point</param>
        /// <param name="timeTaken">Specify time calculated externally for the point</param>
        public void AddPoint(string name, TimeSpan timeTaken)
        {
            var now = _stopWatch.Elapsed;
            _points.Add((name, timeTaken));
            _last = now;
        }

        /// <summary>
        /// Stop the timer and log the message. Disposes the instance.
        /// </summary>
        public void StopAndLog()
        {
            Dispose();
        }

        private void LogMessage(TimeSpan total)
        {
           _logger.Info($"Call: '{_message}' total execute time {total.TotalMilliseconds} ms");

            if (_points.Count > 0)
            {
                foreach (var point in _points)
                {
                    var sm = point.Elapsed.TotalMilliseconds;

                    _logger.Info($"{point.Name} {sm}");
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// Disposable implementation. Will log the collected data on the first call
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                _stopWatch.Stop();
                LogMessage(_stopWatch.Elapsed);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose implementation. Will log the collected data on the first call
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
