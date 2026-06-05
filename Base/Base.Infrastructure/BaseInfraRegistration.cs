using Base.Application.Contracts;
using Base.Infrastructure.Implementation;
using Base.Infrastructure.Implementation.MinIoService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using StackExchange.Redis;
using System.Reflection;

namespace Base.Infrastructure
{
    public static class BaseInfraRegistration
    {
        public static void RegisterBaseInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IJwtManager, JwtManager>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddSingleton<IConnectionMultiplexer>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var redisConnectionString = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

            //services.AddSingleton<RedisService>(provider =>
            //{
            //    var configuration = provider.GetRequiredService<IConfiguration>();
            //    string redisConnectionString = configuration["Redis:ConnectionString"];
            //    return new RedisService(redisConnectionString);
            //});

            services.AddScoped<IRedisService, RedisService>();

            services.AddSingleton<IMinioClient>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();

                var endpoint = configuration["Minio:Endpoint"] ?? "localhost:9000";
                var accessKey = configuration["Minio:AccessKey"] ?? "minioadmin";
                var secretKey = configuration["Minio:SecretKey"] ?? "minioadmin";
                var useSSL = bool.TryParse(configuration["Minio:UseSSL"], out var ssl) && ssl;

                var client = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(useSSL)
                    .Build();

                return client;
            });

            services.AddScoped<IMinIoService, MinIoService>();
            services.AddHttpClient();
        }

        #region AddRepositories
        public static void AddRepositories(
            this IServiceCollection services,
            Assembly implementationAssembly)
        {
            // گرفتن همه ریپازیتوری ها از لایه اینفرا
            var repositoryTypes = implementationAssembly.GetTypes()
                .Where(type => !type.IsAbstract
                               && !type.IsInterface
                               && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRepository<,>)));

            // filter out RepositoryBase<>
            var nonBaseRepos = repositoryTypes.Where(t => t != typeof(Repository<,>));

            foreach (var repositoryType in nonBaseRepos)
            {
                var interfaces = repositoryType.GetInterfaces()
                    .Except(repositoryType.BaseType.GetInterfaces())
                    .ToList();

                if (interfaces.Count != 1)
                    throw new InvalidOperationException($"Repository '{repositoryType.Name}' must implement only one interface that implements IRepositoryBase<T>.");

                services.AddScoped(interfaces[0], repositoryType);
            }
        }
        #endregion
    }
}