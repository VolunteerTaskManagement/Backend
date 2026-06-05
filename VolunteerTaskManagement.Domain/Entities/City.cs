using Base.Domain.Entities.Common;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class City : BaseEntity
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }

        public long ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}
