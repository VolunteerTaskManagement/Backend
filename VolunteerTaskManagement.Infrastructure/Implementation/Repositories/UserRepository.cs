using Base.Infrastructure.Implementation;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Application.Contracts.Repositories;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Infrastructure.Implementation.Repositories
{
    public class UserRepository(IVolunteerTaskManagementContext dbContext)
        : Repository<User, IVolunteerTaskManagementContext>(dbContext), IUserRepository
    {
    }
}
