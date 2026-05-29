using Base.Application.Contracts;
using Base.Infrastructure.Persistence.Context;
using VolunteerTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Context
{
    public class VolunteerTaskManagementContext(DbContextOptions<VolunteerTaskManagementContext> options, IJwtManager jwtManger) 
        : BaseContext(options, typeof(User).Assembly, typeof(VolunteerTaskManagementContext).Assembly, jwtManger)
        , IVolunteerTaskManagementContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.SeedAdmin();
        }
    }
}