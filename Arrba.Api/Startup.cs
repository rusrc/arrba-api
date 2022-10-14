using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using Arrba.Api.ConfigurationExtensions;
using Arrba.Domain;
using Arrba.Extensions;
using Arrba.ImageLibrary;
using Arrba.Middleware;
using Arrba.Repositories;
using Arrba.Repositories.EntityFramework.PostgreSQL;
using Arrba.Services;
using Arrba.Services.Configuration;
using Arrba.Services.Logger;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Arrba.Api
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_specificOriginsForArrba";

        /// <summary>
        /// Startup
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddTransient<ILogService, LogService>();
            services.AddSingleton<IDeclineService, RussianDeclineService>();

            services.AddDbArrbaContext(Configuration);
            services.AddIdentityForDbArrbaContext();
            services.AddJwtAuthentication();
            services.AddSwaggerDocumentation();
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<ApplicationConfiguration>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICaptchaService, GrecaptchaService>();

            var provider = services.BuildServiceProvider();
            var env = provider.GetService<IHostingEnvironment>();

            services.AddTransient(s => new ImgManager(env.WebRootPath));
            services.AddTransient<IImgManager, ImgManagerFirebase>();

            services.AddArrbaCors();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                   new CultureInfo("ru-RU"),
                   new CultureInfo("ru"),
                   //new CultureInfo("en-US"),
                   //new CultureInfo("en"),
                };

                options.DefaultRequestCulture = new RequestCulture("ru-RU");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            if (!env.IsDevelopment())
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(60);
                });

                services.AddResponseCompression();
                services.Configure<GzipCompressionProviderOptions>
                    (options =>
                    {
                        options.Level = CompressionLevel.Fastest;
                    });
            }

            // TODO maybe it is unnecessary
            // https://docs.microsoft.com/ru-ru/aspnet/core/performance/caching/middleware?view=aspnetcore-2.2
            services.AddResponseCaching(options =>
            {
                options.UseCaseSensitivePaths = true;
                // options.MaximumBodySize = 1024 * 2;
            });
        }

        /// <summary>
        /// Configre
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DbArrbaContext context)
        {
            // loggerFactory.AddProvider(null);

            // TODO Check is it used on release mode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseHttpsRedirection();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (!context.AllMigrationsApplied())
            {
                context.Database.Migrate();
            }

            app.UseSwaggerDocumentation();

            app.UseCors(AllowSpecificOrigins);
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlingMiddleware>(app.ApplicationServices.GetService<ILogService>());

            // https://docs.microsoft.com/ru-ru/aspnet/core/fundamentals/portable-object-localization?view=aspnetcore-2.2
            app.UseRequestLocalization();

            // TODO maybe it is unnecessary
            // https://docs.microsoft.com/ru-ru/aspnet/core/performance/caching/middleware?view=aspnetcore-2.2
            app.UseResponseCaching();
            app.Use(async (httpContext, next) =>
            {
                // For GetTypedHeaders, add: using Microsoft.AspNetCore.Http;
                httpContext.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(60)
                    };
                httpContext.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseMvc();
        }
    }
}
