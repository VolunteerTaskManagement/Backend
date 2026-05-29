using Base.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Application.Contracts.Repositories
{
    public interface INeighborhoodRepository : IRepository<Neighborhood, IVolunteerTaskManagementContext>
    {
    }
}
