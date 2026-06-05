using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities.State
{
    public class CancelledState : TaskState
    {
        public override VolunteerTaskStatus Status =>
       VolunteerTaskStatus.Cancelled;
    }
}
