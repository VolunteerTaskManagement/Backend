using Base.Infrastructure.Persistence.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class ProvinceConfig : BaseEntityConfig<Province>
    {
        public override void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces", "VolunteerTaskManagement");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("عنوان");
        }
    }
}