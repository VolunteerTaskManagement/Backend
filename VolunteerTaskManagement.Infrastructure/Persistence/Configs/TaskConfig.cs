using Base.Infrastructure.Persistence.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using VolunteerTaskManagement.Domain.Entities.State;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class TaskConfig : BaseEntityConfig<Domain.Entities.VolunteerTask>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.VolunteerTask> builder)
        {
            builder.ToTable("Tasks", "VolunteerTaskManagement");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("عنوان");

            builder.HasOne(x => x.Neighborhood)
                .WithMany()
                .HasForeignKey(x => x.NeighborhoodId);

            builder
                .Property(o => o.State)
                .HasConversion(
                    s => s.GetType().Name,
                    s => GetTaskState(s)
                                            );
        }



        private static TaskState GetTaskState(string state)
        {
            return state switch
            {
                nameof(RegisterdState) => new RegisterdState(),
                nameof(ConfirmedState) => new ConfirmedState(),
                nameof(InProgressState) => new InProgressState(),
                nameof(CancelledState) => new CancelledState(),
                _ => throw new InvalidOperationException($"Unknown state: {state}")
            };
        }

    }
}




