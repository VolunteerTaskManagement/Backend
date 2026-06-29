using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using VolunteerTaskManagement.Domain.Entities;
using Base.Domain.Entities.Common;
using Base.Utilities.Extensions;
using VolunteerTaskManagement.Domain.Enums;
using Utilities;



namespace VolunteerTaskManagement.Application.CQRS.Profile
{
    public class ProfileDto
    {
        public long Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? UserName { get; set; }
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Role { get; set; }

        public string? PicName { get; set; }
        public string? PicUrl { get; set; }

        public string? NeighborhoodTitle { get; set; }

        public string? NationalCode { get; set; }
        public List<Skill> Skills { get; set; } = [];
        public List<string> SkillTitles =>
            Skills?.Select(x => x.GetDescription()).ToList() ?? new List<string>();
        public static Expression<Func<User, ProfileDto>> Selector =>
            model => new ProfileDto
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                Role = model.Role,
                PicName = model.PicName,
                NeighborhoodTitle = model.Neighborhood.Title,
                Skills=model.Skills ,
                NationalCode = model.NationalCode

            };
    }
}