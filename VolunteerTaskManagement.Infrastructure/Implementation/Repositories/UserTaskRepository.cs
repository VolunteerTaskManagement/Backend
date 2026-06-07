using Base.Infrastructure.Implementation;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Application.Contracts.Repositories;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Infrastructure.Implementation.Repositories
{
    public class UserTaskRepository(IVolunteerTaskManagementContext dbContext) 
        : Repository<UserTask, IVolunteerTaskManagementContext>(dbContext), IUserTaskRepository
    {
    }
}
