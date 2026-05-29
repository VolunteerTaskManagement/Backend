using Base.Domain.Entities.Common;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class Neighborhood : BaseEntity
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }

        public long RegionId { get; set; }
        public Region Region { get; set; }
    }
}
