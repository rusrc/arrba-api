using System;
using Arrba.Api.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Arrba.Api.ConfigurationExtensions
{
    public static class AuthenticationServiceExtension
    {
        const string SigningSecurityKey = "t4uK3S0fCJDFcJVpUr2dgNO4PrHBIiKy8vZx83DGt7ouzEkuP";
        const string JwtSchemeName = "JwtBearer";

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var signingKey = new SigningSymmetricKey(SigningSecurityKey);
            services.AddTransient<IJwtSigningEncodingKey>(s => signingKey)
                .AddTransient<JwtManager>();

            var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;

            services.AddAuthentication(ConfigureOptions)
                .AddJwtBearer(JwtSchemeName, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingDecodingKey.GetKey(),

                        ValidateIssuer = true,
                        ValidIssuer = "ArrbaApplication",

                        ValidateAudience = true,
                        ValidAudience = "ArrbaApplicationClient",
                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                }); 

            return services;
        }

        //public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        //{

        //    return app;
        //}

        private static void ConfigureOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtSchemeName;
            options.DefaultChallengeScheme = JwtSchemeName;
        }
    }
}
