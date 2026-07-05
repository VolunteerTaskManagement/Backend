using Base.Utilities.Extensions;
using System.Linq.Expressions;
using Utilities;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskListDTO
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<Skill> Skills { get; set; } = [];
        public List<string> SkillTitles =>
            Skills?.Select(x => x.GetDescription()).ToList() ?? [];
        public int Count { get; set; }
        public int VolunteerCount { get; set; }
        public string? PicName { get; set; }
        public string? PicUrl { get; set; }
        public string? NeighborhoodTitle { get; set; }
        public string? Address { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateFa => StartDate.ToPersianDateTime().ToString();
        public string? RegionName { get; set; }
        public string? CityName { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsConfirmedByVolunteer { get; set; }
        public VolunteerTaskStatus Status { get; set; }
        public string StatusTitle => Status.GetDescription();
        public double Lat { get; set; }
        public double Lng { get; set; }

        public static Expression<Func<VolunteerTask, TaskListDTO>> Selector(long userId, string role) =>
            model => new TaskListDTO
            {
                Id = model.Id,
                Count = model.Count,
                Description = model.Description,
                NeighborhoodTitle = model.Neighborhood.Title,
                Title = model.Title,
                Lat = model.Lat,
                Lng = model.Lng,
                PicName = model.PicName,
                Skills = model.Skills,
                Address = model.Address,
                StartDate = model.StartDate,
                VolunteerCount = model.VolunteerCount,
                CityName = model.Neighborhood.Region.City.Title,
                RegionName = model.Neighborhood.Region.Title,
                IsAssigned = model.UserTasks.Any(x => x.VolunteerId == userId),
                IsConfirmedByVolunteer = role == "Volunteer" ? model.UserTasks.Where(x => x.VolunteerId == userId).Select(x => x.IsCompleted).FirstOrDefault()
                                                             : !model.UserTasks.Any(x => x.IsCompleted == false),
                Status = model.Status,
            };
    }
}
