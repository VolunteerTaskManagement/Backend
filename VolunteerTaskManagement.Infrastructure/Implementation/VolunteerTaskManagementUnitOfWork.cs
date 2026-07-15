using Base.Infrastructure.Implementation;
using VolunteerTaskManagement.Application.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Infrastructure.Implementation
{
    public class VolunteerTaskManagementUnitOfWork(IVolunteerTaskManagementContext context, IServiceProvider serviceProvider)
        : UnitOfWork<IVolunteerTaskManagementContext>(context), IVolunteerTaskManagementUnitOfWork
    {
        public IUserRepository Users => serviceProvider.GetService<IUserRepository>();
        public INeighborhoodRepository Neighborhoods => serviceProvider.GetService<INeighborhoodRepository>();
        public ITaskRepository Tasks => serviceProvider.GetService<ITaskRepository>();
        public IUserTaskRepository UserTasks => serviceProvider.GetService<IUserTaskRepository>();
        public INotificationLogRepository NotificationLogs => serviceProvider.GetService<INotificationLogRepository>();
    }
}