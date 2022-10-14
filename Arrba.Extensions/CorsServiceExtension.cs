using Microsoft.Extensions.DependencyInjection;
using static Arrba.Constants.CONSTANT;

namespace Arrba.Extensions
{
    public static class CorsServiceExtension
    {
        /// <summary>
        /// http://localhost:4200/
        /// http://localhost:4000/
        /// </summary>
        /// <param name="services"></param>
        /// <param name="allowSpecificOrigins"></param>
        /// <returns></returns>
        public static IServiceCollection AddArrbaCors 
            (this IServiceCollection services, string allowSpecificOrigins = "_specificOriginsForArrba")
        {
            services.AddCors(options =>
            {
                options.AddPolicy(allowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(
                                "http://unix:" + UNIX_HOST,
                                "http://localhost:4200/",
                                "http://localhost:4000/")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });

            return services;
        }
    }
}
