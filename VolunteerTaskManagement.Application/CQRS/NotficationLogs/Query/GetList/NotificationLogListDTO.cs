using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Utilities;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Application.CQRS.NotficationLogs.Query.GetList
{
    public class NotificationLogListDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// متن نوتیف
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// نوع
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// کاربران هدف
        /// </summary>
        public List<long> UsersId { get; set; } = [];

        /// <summary>
        /// دیده شده
        /// </summary>
        public bool IsSeen { get; set; }

        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public string? CreateDateFa { get; set; }

        public static Expression<Func<NotificationLog, NotificationLogListDTO>> Selector =>
            model => new NotificationLogListDTO
            {
                Title = model.Title,
                Id = model.Id,
                UsersId = model.UsersId,
                Type = model.Type,
                IsSeen = model.IsSeen,
                CreateDateFa = model.CreateDate.ToPersianDateTime().ToString()
            };
    }
}
