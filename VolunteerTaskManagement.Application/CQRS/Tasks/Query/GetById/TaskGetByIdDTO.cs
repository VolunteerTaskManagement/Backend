using Base.Utilities.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Utilities;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskGetByIdDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public List<Skill> Skills { get; set; } = [];
        public List<string> SkillTitles =>
            Skills?.Select(x => x.GetDescription()).ToList() ?? [];
        public string? PicName { get; set; }
        public string? PicUrl { get; set; }
        public int Count { get; set; }
        public int VolunteerCount { get; set; }

        public string? Description { get; set; }
        public string? Mobile { get; set; }
        public string? CoordinatorName { get; set; }
        public long? CreatorId { get; set; }

        public string? Address { get; set; }

        public DateTime StartDate { get; set; }
        public string StartDateFa => StartDate.ToPersianDateTime().ToShortDateString();
        public long NeighborhoodId { get; set; }
        public string? NeighborhoodTitle { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsConfirmedByVolunteer { get; set; }
        public VolunteerTaskStatus Status { get; set; }
        public string StatusTitle => Status.GetDescription();

        [Display(Name = "عرض جغرافیایی")]
        public double Lat { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public double Lng { get; set; }

        public static Expression<Func<VolunteerTask, TaskGetByIdDTO>> Selector(long userId, string role) =>
            model => new TaskGetByIdDTO
            {
                Id = model.Id,
                CreatorId = model.CreatedBy,
                Count = model.Count,
                Description = model.Description,
                NeighborhoodTitle = model.Neighborhood.Title,
                Title = model.Title,
                PicName = model.PicName,
                Skills = model.Skills,
                Lat = model.Lat,
                Lng = model.Lng,
                Status = model.Status,
                Address = model.Address,
                StartDate = model.StartDate,
                VolunteerCount = model.VolunteerCount,
                IsAssigned = model.UserTasks.Any(x => x.VolunteerId == userId),
                IsConfirmedByVolunteer = role == "Volunteer" ? model.UserTasks.Where(x => x.VolunteerId == userId).Select(x => x.IsCompleted).FirstOrDefault()
                                                             : !model.UserTasks.Any(x => x.IsCompleted == false)
            };
    }
}
