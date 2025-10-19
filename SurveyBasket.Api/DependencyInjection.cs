using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using SurveyBasket.Contracts;
using SurveyBasket.Core.Interfaces;
using SurveyBasket.Infrastructure.Services;
using System.Reflection;

namespace SurveyBasket.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddOpenApiDependencies()
                .AddEntitiesServicesDependencies()
                .AddMapsterConfig()
                .AddFluentValidationConfig();

            return services;
        }

        public static IServiceCollection AddEntitiesServicesDependencies(this IServiceCollection services)
        {

            services.AddScoped<IPollService, PollService>();

            return services;
        }

        public static IServiceCollection AddOpenApiDependencies(this IServiceCollection services)
        {
           
            services.AddOpenApi();

            return services;
        }


        public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {

            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            return services;
        }

        public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {

            services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            services.AddFluentValidationAutoValidation();

            return services;
        }
    }
}
