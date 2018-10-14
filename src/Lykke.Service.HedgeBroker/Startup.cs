﻿using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.HedgeBroker.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using AutoMapper;
using Lykke.Service.HedgeBroker.Middleware;

namespace Lykke.Service.HedgeBroker
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "HedgeBroker API",
            ApiVersion = "v1"
        };

        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<AppSettings>(options =>
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfiles(typeof(AutoMapperProfile));
                });

                Mapper.AssertConfigurationIsValid();

                options.SwaggerOptions = _swaggerOptions;
                
                options.Logs = logs =>
                {
                    logs.AzureTableName = "HedgeBrokerLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.HedgeBrokerService.Db.LogsConnectionString;
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.WithMiddleware = builder =>
                {
                    builder.UseApiExceptionsMiddleware();
                };
            });
        }
    }
}
