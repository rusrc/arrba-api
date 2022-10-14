using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Extensions;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arrba.Api.Controllers
{
    /// <summary>
    /// Data migration
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DataMigrationController : ControllerBase
    {

        private readonly ILogService _logService;
        private readonly IHostingEnvironment _environment;
        private readonly ApplicationConfiguration _applicationConfiguration;

        #pragma warning disable CS1591
        public DataMigrationController(
            ILogService logService, 
            IHostingEnvironment environment, 
            ApplicationConfiguration applicationConfiguration)
        {
            _logService = logService;
            _environment = environment;
            _applicationConfiguration = applicationConfiguration;
        }

        [HttpGet]
        [Route("TestPostgreSqlCertWork", Name = "Test the postgresql sertificate work.")]
        public async Task<ActionResult<bool>> GetTest(string password, bool seedData = false)
        {
            if (password != "123456Ru!")
            {
                return BadRequest();
            }

            var connection = this._applicationConfiguration.DbArrbaConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<DbArrbaContext>();

            optionsBuilder.UseNpgsql(connection, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                builder.RemoteCertificateValidationCallback((s, c, ch, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                    {
                        return true;
                    }

                    _logService.Error($@"Certificate error: {sslPolicyErrors}, 
                        Do not allow this client to communicate with unauthenticated servers");

                    return false;
                });
                builder.ProvideClientCertificatesCallback(clientCerts =>
                {
                    var clientCertPath = "/home/kursoft/.postgresql/root.crt";
                    var certExists = System.IO.File.Exists(clientCertPath);

                    _logService.Info($"Cert exists {certExists}");

                    // To avoid permission ex run: "sudo chmod -R 777 /home/kursoft/.postgresql/root.crt"
                    var cert = new X509Certificate2(clientCertPath);
                    clientCerts.Add(cert);
                });
            });

            using (var ctx = new DbArrbaContext(optionsBuilder.Options))
            {
                var canConnect = await ctx.Database.CanConnectAsync();
                _logService.Info($"Can connect {canConnect}");

                if (!ctx.AllMigrationsApplied())
                {
                    await ctx.Database.MigrateAsync();
                }

                if (seedData)
                {
                    var pathFile =
                        Path.Combine(_environment.ContentRootPath, "Data", "migrations.sql");
                    ctx.Database.Seed(pathFile);
                }

                return await ctx.Database.CanConnectAsync();
            }
        }

        internal static byte[] ReadFile(string fileName)
        {
            using (var f = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int size = (int)f.Length;
                byte[] data = new byte[size];
                size = f.Read(data, 0, size);
                f.Close();
                return data;
            }
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // _logService.Info($"sslPolicyErrors {sslPolicyErrors}");

            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            //_logService.Error($@"Certificate error: {sslPolicyErrors}, 
            //    Do not allow this client to communicate with unauthenticated servers");

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        private void ProvideClientCertificates(X509CertificateCollection clientCerts)
        {
            var clientCertPath = "/home/kursoft/.postgresql/root.crt";
            var certExists = System.IO.File.Exists(clientCertPath);

            _logService.Info($"Cert exists {certExists}");

            // To avoid permission ex run: "sudo chmod -R 777 /home/kursoft/.postgresql/root.crt"
            var cert = new X509Certificate2(clientCertPath);
            clientCerts.Add(cert);
        }
    }
}
