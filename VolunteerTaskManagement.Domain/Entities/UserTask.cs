using Base.Domain.Entities.Common;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class UserTask : BaseEntity
    {
        public long VolunteerId { get; set; }
        public User Volunteer { get; set; }

        public long TaskId { get; set; }
        public Task Task { get; set; }
    }
}
