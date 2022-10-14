using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using static Arrba.Constants.CONSTANT;

namespace Arrba.Api
{
    /// <summary>
    /// The program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main endpoint
        /// </summary>
        public static void Main(string[] args)
        {
            // https://github.com/aspnet/KestrelHttpServer/issues/2104
            const int workerThreads = 256;

            // System.Net.ServicePointManager.DefaultConnectionLimit = workerThreads; // Max concurrent outbound reques$ not used in dotnet core 2.*
            ThreadPool.GetMaxThreads(out int _, out int completionThreads);
            ThreadPool.SetMinThreads(workerThreads, completionThreads); // or higher

            WebHost.CreateDefaultBuilder(args)
               .UseLibuv()
               .ConfigureKestrel(options =>
               {
                   options.Limits.MaxRequestLineSize = 16384;
                   // nginx.org/ru/docs/http/ngx_http_upstream_module.html
                   // nginx.org/ru/docs/stream/ngx_stream_upstream_module.html
                   options.ListenUnixSocket(UNIX_HOST);
               })
                // .UseUrls("http://*:80")
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    //logging.AddProvider()
                })
                .Build()
                .Run();
        }
    }
}
