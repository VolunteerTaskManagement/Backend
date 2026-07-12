using Base.Infrastructure.Implementation;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Application.Contracts.Repositories;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Infrastructure.Implementation.Repositories
{
    public class NotificationLogRepository(IVolunteerTaskManagementContext dbContext)
        : Repository<NotificationLog, IVolunteerTaskManagementContext>(dbContext), INotificationLogRepository
    {
    }
}
