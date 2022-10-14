using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Arrba.ImageLibrary;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Arrba.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransient<ILogService, DebugLogService>();

            var environmentName = Environment.GetEnvironmentVariable("Development");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            IConfiguration cofiguration = builder.Build();

            services.AddTransient(s => new ApplicationConfiguration(cofiguration));

            var rootPath = Environment.CurrentDirectory;
            services.AddTransient(s => new ImgManagerSkiaSharp(rootPath, s.GetService<ILogService>()));

            var provider = services.BuildServiceProvider();
            var imgManager = provider.GetService<ImgManagerSkiaSharp>();

            var timer = Stopwatch.StartNew();

            var imgJson = imgManager.SaveImagesAsync("TestCategory", "SourceFolder");

            Task.WaitAll(imgJson);

            timer.Stop();

            System.Console.WriteLine(timer.ElapsedMilliseconds);
            System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(imgJson));
            System.Console.ReadKey();
        }
    }
}
