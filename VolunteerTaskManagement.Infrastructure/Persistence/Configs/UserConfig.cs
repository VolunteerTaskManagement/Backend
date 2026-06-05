using Base.Infrastructure.Persistence.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Infrastructure.Persistence.Configs
{
    public class UserConfig : BaseEntityConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "VolunteerTaskManagement");

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("نام");

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("نام خانوادگی");

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasComment("نام کاربری");

            builder.Property(x => x.Email)
                .HasMaxLength(60)
                .HasComment("آدرس ایمیل");

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasComment("هش پسوورد");

            builder.Property(x => x.Role)
                .HasComment("نقش");

            builder.Property(x => x.RefreshToken)
                .HasComment("رفرش توکن");

            builder.Property(x => x.RefreshTokenExpiryTime)
                .HasComment("انقضای رفرش توکن");

            builder.Property(x => x.BirthDate)
                .HasComment("تاریخ تولد");

            builder.Property(x => x.PicName)
                .HasMaxLength(100)
                .HasComment("نام فایل تصویر پروفایل");

            builder.Property(x => x.PhoneNumber)
                .HasComment("شماره تلفن");

            builder.HasOne(x => x.Neighborhood)
                  .WithMany()
                  .HasForeignKey(x => x.NeighborhoodId);

            builder.Property(x => x.Skills)
                   .HasComment("مهارت ها");




            //builder.HasIndex(x => x.Email)
            //    .IsUnique()
            //    .HasFilter("[Email] IS NOT NULL");
        }
    }
}