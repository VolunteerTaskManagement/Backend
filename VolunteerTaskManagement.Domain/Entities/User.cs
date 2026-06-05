using System.ComponentModel;
using Base.Domain.Entities.Common;

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

        public ICollection<UserTask> UserTasks { get; set; }
    }
}