using System.Reflection;
using Base.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Infrastructure.Implementation;
using VolunteerTaskManagement.Infrastructure.Persistence.Context;

namespace VolunteerTaskManagement.Infrastructure
{
    public static class VolunteerTaskManagementInfraRegistration
    {
        public static void RegisterVolunteerTaskManagementInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VolunteerTaskManagementContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IVolunteerTaskManagementContext, VolunteerTaskManagementContext>();
            services.AddRepositories(Assembly.GetExecutingAssembly());
            services.AddScoped<IVolunteerTaskManagementUnitOfWork, VolunteerTaskManagementUnitOfWork>();
            //services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        }
    }
}