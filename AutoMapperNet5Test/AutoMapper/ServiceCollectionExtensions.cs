namespace AutoMapperNet5Test.AutoMapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ServiceCollectionExtensions
    {
        public const string ServiceModels = "AutoMapperNet5Test";
        public const string ServerModels = "AutoMapperNet5Test";
        public const string PubSubModels = "AutoMapperNet5Test";
        public const string ApiBusinessModels = "AutoMapperNet5Test";
        public const string ApiServiceModels = "AutoMapperNet5Test";

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mappingAssemblies = new[]
            {
                ServiceModels,
                ServerModels,
                PubSubModels,
                ApiBusinessModels,
                ApiServiceModels,
            };

            var configuration = new MapperConfiguration(config => config.RegisterMappingsFrom(mappingAssemblies));

            var mapper = new Mapper(configuration);
            services.AddSingleton(mapper);
            return services;
        }

        public static IApplicationBuilder UseAutoMapper(this IApplicationBuilder app,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                AutoMapperSingleton.Init(app.ApplicationServices.GetService<Mapper>());
            });

            return app;
        }
    }
}