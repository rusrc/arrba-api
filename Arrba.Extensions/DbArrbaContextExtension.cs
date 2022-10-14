using System;
using System.Data;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Arrba.Domain;
using Arrba.Services.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Arrba.Extensions
{
    /// <summary>
    /// Postgresql parameters:
    /// https://www.npgsql.org/doc/connection-string-parameters.html
    ///
    /// Issues
    /// Error: "Attempted to read past the end of the stream"
    /// Occored when used PbBouncer in TRANSACTION mode.
    /// Fix: Add in connection `No Reset On Close = true`
    /// Readmore: https://github.com/npgsql/npgsql/issues/2221
    ///           https://www.npgsql.org/doc/performance.html#pooled-connection-reset
    /// </summary>
    public static class DbArrbaContextExtension
    {
        public static IServiceCollection AddDbArrbaContext
            (this IServiceCollection services, IConfiguration configuration, bool useCluster = true)
        {
            var provider = services.BuildServiceProvider();
            var logService = provider.GetService<ILogService>();

            Npgsql.Logging.NpgsqlLogManager.Provider = new NpgSqlLoggingProvider(logService);

            if (useCluster)
            {
                var connectionString = configuration.GetConnectionString("DbArrbaCluster");
                logService.Info($"Use DbArrbaCluster connection: ${connectionString.Substring(0, 19)}");

                services.AddDbContext<DbArrbaContext>(options =>
                    options.UseNpgsql(CreateConnection(connectionString, logService), builder =>
                    {
                        builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), new[] { "EnableRetryOnFailure" });
                        builder.RemoteCertificateValidationCallback(ValidateServerCertificate);
                        builder.ProvideClientCertificatesCallback(ProvideClientCertificates);
                    }));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DbArrba");
                logService.Info($"Use local DbArrba connection: {connectionString.Substring(0, 19)}");
                services.AddDbContext<DbArrbaContext>(options =>
                    options.UseNpgsql(CreateConnection(connectionString, logService)));
            }

            return services;
        }

        public static void Seed(this DatabaseFacade databaseFacade, string filePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Seeding file not found, path: {filePath}");
            }

            var rawSqlString = File.ReadAllText(filePath);

            using (var command = databaseFacade.GetDbConnection().CreateCommand())
            {
                command.CommandText = rawSqlString;
                command.CommandType = CommandType.Text;

                databaseFacade.OpenConnection();

                using (var result = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    // add the result display if needed `var result = command.ExecuteReader(...)`
                }
            }
        }

        #region Helpers
        private static NpgsqlConnection CreateConnection(string connectionString, ILogService logService)
        {
            var connection = new NpgsqlConnection(connectionString);

#if DEBUG
            // connection.StateChange += (sender, args) =>
            // {
            //     logService.Info($"ConnectionState: {args.OriginalState}. Current time: {DateTime.Now.ToShortTimeString()}");
            // };
#endif

            return connection;
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        private static void ProvideClientCertificates(X509CertificateCollection clientCerts)
        {
            var clientCertPath = "/home/kursoft/.postgresql/root.crt";
            var certExists = File.Exists(clientCertPath);

            if (!certExists)
            {
                throw new FileNotFoundException($"Certificate file by path '{clientCertPath}' not found.");
            }

            // To avoid permission ex run: "sudo chmod -R 777 /home/kursoft/.postgresql/root.crt"
            var cert = new X509Certificate2(clientCertPath);
            clientCerts.Add(cert);
        } 
        #endregion
    }
}
