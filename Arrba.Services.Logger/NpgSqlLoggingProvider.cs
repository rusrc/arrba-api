using System;
using Npgsql.Logging;

namespace Arrba.Services.Logger
{
    /// <summary>
    /// Source documentation https://www.npgsql.org/doc/logging.html
    /// </summary>
    public class NpgSqlLoggingProvider : INpgsqlLoggingProvider
    {
        private readonly ILogService _logService;

        public NpgSqlLoggingProvider(ILogService logService)
        {
            this._logService = logService;
        }

        public NpgsqlLogger CreateLogger(string name)
        {
            return new NpgsqlArrbaLogger(_logService);
        }
    }

    internal class NpgsqlArrbaLogger : NpgsqlLogger
    {
        public ILogService LogService { get; }

        public NpgsqlArrbaLogger(ILogService logService)
        {
            this.LogService = logService;
        }

        public override bool IsEnabled(NpgsqlLogLevel level)
        {
            return true; // all levels
        }

        public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception exception = null)
        {
            // Level: Debug, connectorId: 1237043804, msg: Connection opened
            // Level: Debug, connectorId: 1237043804, msg: Executing statement(s):\n UPDATE "AdVehic...
            // Level: Debug, connectorId: 1237043804, msg: Connection closed

            if (msg.Contains("Connection opened") || msg.StartsWith("Executing statement") || msg.Contains("Connection closed"))
            {
                this.LogService
                    .Debug($"POSTGRESQL Level: {level}, connectorId: {connectorId}, msg: {msg.Substring(0, Math.Min(msg.Length, 250))}", exception);
            }
        }
    }
}
