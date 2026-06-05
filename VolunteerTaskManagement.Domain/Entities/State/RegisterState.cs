using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities.State
{
    public class RegisterdState : TaskState
    {
        public override VolunteerTaskStatus Status =>
     VolunteerTaskStatus.Registered;
        public override void Start(VolunteerTask task)
        { 
            task.State = new InProgressState();
        }

        public override void Cancel(VolunteerTask task)
        {
            task.State = new CancelledState();
        }
    }
}
