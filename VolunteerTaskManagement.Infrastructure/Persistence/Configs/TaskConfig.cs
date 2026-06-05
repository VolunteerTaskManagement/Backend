using Base.Infrastructure.Persistence.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        }
    }
}