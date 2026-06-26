using Base.Utilities.Extensions;
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
        public string StartDateFa => StartDate.ToPersianDateTime().ToString();
        public long NeighborhoodId { get; set; }
        public string? NeighborhoodTitle { get; set; }

        public static Expression<Func<VolunteerTask, TaskGetByIdDTO>> Selector =>
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
                Address = model.Address,
                StartDate = model.StartDate,
                VolunteerCount = model.VolunteerCount,
            };
    }
}
