﻿using Batch4.Api.FitnessTracker.Db;
using Batch4.Api.FitnessTracker.Features.User;
using Microsoft.EntityFrameworkCore;

namespace Batch4.Api.FitnessTracker.Services
{
    public static class ModularService
    {
        public static IServiceCollection AddServices(this IServiceCollection services,WebApplicationBuilder builder)
        {
            builder.Services.AddAppDbContextService(builder);
            builder.Services.AddDataAcessService();
            builder.Services.AddBusinessLogicService();
            return services;
        }

        public static IServiceCollection AddAppDbContextService(this IServiceCollection services,WebApplicationBuilder builder)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);
            return services;
        }

        public static IServiceCollection AddDataAcessService(this  IServiceCollection service)
        {
            service.AddScoped<DA_User>();
            return service;
        }

        public static IServiceCollection AddBusinessLogicService(this IServiceCollection service)
        {
            service.AddScoped<BL_User>();
            return service;
        }
    }
}