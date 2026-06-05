using Microsoft.EntityFrameworkCore;
using Base.Infrastructure.Persistence.Configs;
using VolunteerTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class NeighborhoodConfig : BaseEntityConfig<Neighborhood>
    {
        public override void Configure(EntityTypeBuilder<Neighborhood> builder)
        {
            builder.ToTable("Neighborhoods", "VolunteerTaskManagement");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("عنوان");

            builder.HasOne(x => x.Region)
                .WithMany()
                .HasForeignKey(x => x.RegionId);
        }
    }
}