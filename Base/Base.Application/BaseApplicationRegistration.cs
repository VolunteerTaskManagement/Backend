using Base.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Reflection;

namespace Base.Application
{
    public static class BaseApplicationRegistration
    {
        public static void RegisterBaseApplication(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var services = builder.Services;

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddFluentValidation(Assembly.GetExecutingAssembly());

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddHttpContextAccessor();

        }

        public static void AddFluentValidation(this IServiceCollection service, Assembly assembly)
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

            service.AddValidatorsFromAssembly(assembly);
        }

        public static void AddMediatR(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        }
    }
}