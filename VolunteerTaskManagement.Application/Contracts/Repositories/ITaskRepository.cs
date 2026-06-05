using Base.Application.Contracts;

namespace VolunteerTaskManagement.Application.Contracts.Repositories
{
    public interface ITaskRepository : IRepository<Domain.Entities.VolunteerTask, IVolunteerTaskManagementContext>
    {
    }
}
