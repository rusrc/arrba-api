using System;
using Microsoft.Extensions.Logging;

namespace Arrba.Admin.LoggerProvider
{
    /// <summary>
    /// Usage in controller
    ///    _context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
    /// </summary>
    public class BaseLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private class MyLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId,
                TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var message = "Log..." + formatter(state, exception);

                System.Diagnostics.Debug.WriteLine(message);
            }
        }
    }
}
