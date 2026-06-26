using Base.Domain.Entities.Common;
using VolunteerTaskManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using VolunteerTaskManagement.Domain.Entities.State;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class VolunteerTask : BaseEntity
    {
        public string Title { get; set; }

        public List<Skill> Skills { get; set; }

        public int Count { get; set; }
        public int VolunteerCount { get; set; }

        public string Description { get; set; }
        
        public string PicName { get; set; }

        public string Address { get; set; }

        public DateTime StartDate { get; set; }

        public long NeighborhoodId { get; set; }
        public Neighborhood Neighborhood { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }

        public TaskState State { get; set; }
        public VolunteerTaskStatus Status => State.Status;

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
