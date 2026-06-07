using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Domain.Entities.State
{
    public abstract class TaskState
    {
        public abstract VolunteerTaskStatus Status { get; }

        public virtual void Confirm(VolunteerTask task)
        {
            throw new InvalidOperationException("تغییر وضعیت مجاز نیست");
        }

        public virtual void Start(VolunteerTask task)
        {
            throw new InvalidOperationException("تغییر وضعیت مجاز نیست");
        }

        public virtual void Cancel(VolunteerTask task)
        {
            throw new InvalidOperationException("تغییر وضعیت مجاز نیست");
        }
    }
}
