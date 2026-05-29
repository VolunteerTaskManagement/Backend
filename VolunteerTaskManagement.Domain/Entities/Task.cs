using Base.Domain.Entities.Common;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }

        public List<Skill> Skills { get; set; }

        public int Count { get; set; }

        // TODO: State Management

        public long NeighborhoodId { get; set; }
        public Neighborhood Neighborhood { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}
