using Base.Infrastructure.Persistence.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class CityConfig : BaseEntityConfig<City>
    {
        public override void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities", "VolunteerTaskManagement");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("عنوان");

            builder.HasOne(x => x.Province)
                .WithMany()
                .HasForeignKey(x => x.ProvinceId);
        }
    }
}