using Microsoft.EntityFrameworkCore;
using Base.Infrastructure.Persistence.Configs;
using VolunteerTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class UserTaskConfig : BaseEntityConfig<UserTask>
    {
        public override void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.ToTable("UserTasks", "VolunteerTaskManagement");

            builder.HasOne(x => x.Volunteer)
                .WithMany(y => y.UserTasks)
                .HasForeignKey(x => x.VolunteerId);

            builder.HasOne(x => x.Task)
                .WithMany(y => y.UserTasks)
                .HasForeignKey(x => x.TaskId);
        }
    }
}