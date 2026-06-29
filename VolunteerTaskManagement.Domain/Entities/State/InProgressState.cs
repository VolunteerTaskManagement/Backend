using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities.State
{
    public class InProgressState : TaskState
    {
        public override VolunteerTaskStatus Status =>
     VolunteerTaskStatus.InProgress;
        public override void Confirm(VolunteerTask task)
        {
            task.State = new ConfirmedState();
            task.Status = VolunteerTaskStatus.Confirmed;

        }

        public override void Cancel(VolunteerTask task)
        {
            task.State = new CancelledState();
            task.Status = VolunteerTaskStatus.Cancelled;

        }
    }
}
