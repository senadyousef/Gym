using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;
using Waffer.Server.Configurations;

namespace Waffer.Server.Extensions
{
    public static class ServicesCollections
    {
        internal static AppConfiguration GetApplicationSettings(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppConfiguration>();
        }


        internal static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, AppConfiguration config)
        {
            var key = Encoding.ASCII.GetBytes(config.Key);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme;
                    authentication.DefaultChallengeScheme = Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme;
                })
                .AddCookie(cfg =>
                {
                    cfg.SlidingExpiration = true;
                    cfg.AccessDeniedPath = "/Accounn/SignIn";
                    cfg.LoginPath = "/Accounn/SignIn";
                })
                .AddJwtBearer(async bearer =>
                {
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,

                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        RoleClaimType = ClaimTypes.Role,

                        ClockSkew = TimeSpan.Zero
                    };

                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounn/SignIn";
                options.AccessDeniedPath = "/Accounn/SignIn";
                options.LogoutPath = "/Accounn/SignOut";
            });
            // services.AddAuthorization();
            return services;
        }
    }
}
