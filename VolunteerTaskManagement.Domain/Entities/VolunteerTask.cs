using Base.Domain.Entities.Common;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class VolunteerTask : BaseEntity
    {
        public string Title { get; set; }

        public List<Skill> Skills { get; set; }

        public int Count { get; set; }

        public string Description { get; set; }
        
        public string PicName { get; set; }

        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        // TODO: State Management

        public long NeighborhoodId { get; set; }
        public Neighborhood Neighborhood { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}
