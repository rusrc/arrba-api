using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Arrba.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int workerThreads = 256;

            // System.Net.ServicePointManager.DefaultConnectionLimit = workerThreads; // Max concurrent outbound reques$ not used in dotnet core 2.*
            ThreadPool.GetMaxThreads(out int _, out int completionThreads);
            ThreadPool.SetMinThreads(workerThreads, completionThreads); // or higher

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://localhost:3001");
    }
}
