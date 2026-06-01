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
        public List<string> SkillTitles => [.. Skills.Select(x => x.GetDescription())];
        public int Count { get; set; }
        public string? PicName { get; set; }
        public string? PicUrl { get; set; }
        public string? NeighborhoodTitle { get; set; }
        public string? Address { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateFa => StartDate.ToPersianDateTime().ToString();
        public string? RegionName { get; set; }
        public string? CityName { get; set; }

        public static Expression<Func<VolunteerTask, TaskListDTO>> Selector =>
            model => new TaskListDTO
            {
                Id = model.Id,
                Count = model.Count,
                Description = model.Description,
                NeighborhoodTitle = model.Neighborhood.Title,
                Title = model.Title,
                PicName = model.PicName,
                Skills = model.Skills,
                Address = model.Address,
                StartDate = model.StartDate,
                CityName = model.Neighborhood.Region.City.Title,
                RegionName = model.Neighborhood.Region.Title,
            };
    }
}
