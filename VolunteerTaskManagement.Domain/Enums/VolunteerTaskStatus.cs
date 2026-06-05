using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VolunteerTaskManagement.Domain.Enums
{
    public enum VolunteerTaskStatus
    {
        [Description("ثبت شده")]
        Registered = 1,

        [Description("در حال انجام")]
        InProgress = 2,

        [Description("به اتمام رسیده")]
        Confirmed = 3,

        [Description("کنسل شده")]
        Cancelled = 4
    }
}
