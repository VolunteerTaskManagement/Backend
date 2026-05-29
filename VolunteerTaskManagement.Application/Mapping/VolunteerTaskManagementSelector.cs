using System.Linq.Expressions;
using Base.Application.Contracts.DTOs;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Application.Mapping
{
    public class VolunteerTaskManagementSelector
    {
        public static Expression<Func<Neighborhood, SelectListDTO>> Neighborhood_To_SelectList
          => model => new SelectListDTO(model.Id, $"{model.Title} - {model.Region.Title}");
    }
}
