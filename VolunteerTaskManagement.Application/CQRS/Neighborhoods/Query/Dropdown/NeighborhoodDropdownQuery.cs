using LinqKit;
using MediatR;
using System.Linq.Expressions;
using Base.Application.Contracts.DTOs;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Application.Mapping;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Neighborhoods
{
    public class NeighborhoodDropdownQuery : IRequest<Result<List<SelectListDTO>>>
    {
        public string Title { get; set; }

        #region توابع
        public Expression<Func<Neighborhood, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<Neighborhood>(true);

            if (!string.IsNullOrEmpty(Title))
                filter.And(x => x.Title.Contains(Title));

            return filter;
        }
        #endregion
    }

    public class NeighborhoodDropdownQueryHandler(IVolunteerTaskManagementUnitOfWork uow)
        : IRequestHandler<NeighborhoodDropdownQuery, Result<List<SelectListDTO>>>
    {
        public async Task<Result<List<SelectListDTO>>> Handle(NeighborhoodDropdownQuery request, CancellationToken cancellationToken)
        {
            var filter = request.GetFilter();

            var model = await uow.Neighborhoods.GetDTOAsync(
                VolunteerTaskManagementSelector.Neighborhood_To_SelectList,
                filter,
                take: 50
                );

            return Result.Success(model);
        }
    }
}
