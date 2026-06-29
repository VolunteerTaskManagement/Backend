using Base.Domain.Entities.Common;
using System.ComponentModel;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        [DisplayName("نام")]
        public string FirstName { get; set; }

        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }

        [DisplayName("نام کاربری")]
        public string UserName { get; set; }

        [DisplayName("ایمیل")]
        public string Email { get; set; }

        [DisplayName("رمز عبور")]
        public string PasswordHash { get; set; }

        [DisplayName("نقش")]
        public string Role { get; set; } = string.Empty;

        public string RefreshToken { get; set; }

        [DisplayName("زمان انقضای رفرش توکن")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [DisplayName("تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        public string PicName { get; set; }

        [DisplayName("شماره تلفن همراه")]
        public string PhoneNumber { get; set; }

        public long? NeighborhoodId { get; set; }

        public Neighborhood Neighborhood { get; set; }

        [DisplayName("مهارت ها ")]
        public List<Skill> Skills { get; set; }

        [DisplayName("کد ملی ")]
        public string NationalCode { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}