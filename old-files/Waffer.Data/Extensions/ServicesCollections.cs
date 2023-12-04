using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Waffer.Domain.Entities.Identity;

namespace Waffer.Data.Extensions
{
    public static class ServicesCollections
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default")));


            return services;
        }


    }
}
