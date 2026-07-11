using VolunteerTaskManagement.Domain.Enums;

namespace Base.Application.Contracts.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpirtTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsProfileComplete { get; set; }
        public long? NeighborhoodId { get; set; }
        public List<Skill> Skills { get; set; } = [];
    }
}