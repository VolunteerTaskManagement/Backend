using Base.Infrastructure.Implementation;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Application.Contracts.Repositories;

namespace VolunteerTaskManagement.Infrastructure.Implementation.Repositories
{
    public class TaskRepository(IVolunteerTaskManagementContext dbContext)
        : Repository<Domain.Entities.VolunteerTask, IVolunteerTaskManagementContext>(dbContext), ITaskRepository
    {
    }
}
