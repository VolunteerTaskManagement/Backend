using Base.Domain.Entities.Common;
using System.ComponentModel;

namespace VolunteerTaskManagement.Domain.Entities
{
    public class NotificationLog : BaseEntity
    {
        /// <summary>
        /// متن نوتیف
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// نوع
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// کاربران هدف
        /// </summary>
        public List<long> UsersId { get; set; }

        /// <summary>
        /// دیده شده
        /// </summary>
        public bool IsSeen { get; set; }
    }
}
