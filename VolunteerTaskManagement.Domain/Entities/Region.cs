using Base.Domain.Entities.Common;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class Region : BaseEntity
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }

        public long CityId { get; set; }
        public City City { get; set; }
    }
}
