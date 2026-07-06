using System.Linq.Expressions;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskGetVolunteerConfirmationsDTO
    {
        public long Id { get; set; }
        public string? VolunteerName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsConfirmed { get; set; }

        public static Expression<Func<UserTask, TaskGetVolunteerConfirmationsDTO>> Selector =>
            model => new TaskGetVolunteerConfirmationsDTO
            {
                Id = model.Id,
                VolunteerName = model.Volunteer.FirstName + ' ' + model.Volunteer.LastName,
                PhoneNumber = model.Volunteer.PhoneNumber,
                IsConfirmed = model.IsCompleted,
            };
    }
}
