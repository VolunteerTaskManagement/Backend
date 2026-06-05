using Base.Application.Contracts;
using VolunteerTaskManagement.Application.Contracts.Repositories;

namespace VolunteerTaskManagement.Application.Contracts
{
    public interface IVolunteerTaskManagementUnitOfWork : IUnitOfWork<IVolunteerTaskManagementContext>, IDisposable
    {
        IUserRepository Users { get; }
        INeighborhoodRepository Neighborhoods { get; }
        ITaskRepository Tasks { get; }
        IUserTaskRepository UserTasks { get; }
    }
}