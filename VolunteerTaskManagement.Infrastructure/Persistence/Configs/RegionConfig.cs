using Microsoft.EntityFrameworkCore;
using Base.Infrastructure.Persistence.Configs;
using VolunteerTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class RegionConfig : BaseEntityConfig<Region>
    {
        public override void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToTable("Regions", "VolunteerTaskManagement");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("عنوان");

            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId);
        }
    }
}