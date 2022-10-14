using Arrba.Domain;
using Arrba.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Arrba.Api.ConfigurationExtensions
{
    public static class AddIdentityExtension
    {
        public static IServiceCollection AddIdentityForDbArrbaContext(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<DbArrbaContext>();

            return services;
        }
    }
}
