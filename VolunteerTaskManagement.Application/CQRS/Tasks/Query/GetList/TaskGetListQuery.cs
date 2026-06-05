using Base.Application;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using LinqKit;
using MediatR;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskGetListQuery : IRequest<Result<ItemListDTO<TaskListDTO>>>
    {
        public string? Title { get; set; }
        public List<long> NeighborhoodIds { get; set; } = [];
        public List<Skill> Skills { get; set; } = [];
        public List<VolunteerTaskStatus> Statuses { get; set; } = [];

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public Expression<Func<VolunteerTask, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<VolunteerTask>(true);

            if (!string.IsNullOrEmpty(Title))
                filter.And(x => x.Title.Contains(Title));

            if (NeighborhoodIds.Count != 0)
                filter.And(x => NeighborhoodIds.Contains(x.Id));

            if (Skills.Count != 0)
                filter.And(x => x.Skills.Any(s => Skills.Contains(s)));

            if (Statuses.Count != 0)
                filter.And(x => Statuses.Contains(x.Status));

            return filter;
        }
    }

    public class TaskGetListQueryHandler(IVolunteerTaskManagementUnitOfWork uow, IMinIoService minIoService)
        : IRequestHandler<TaskGetListQuery, Result<ItemListDTO<TaskListDTO>>>
    {
        public async Task<Result<ItemListDTO<TaskListDTO>>> Handle(TaskGetListQuery request, CancellationToken cancellationToken)
        {
            var sort = "id desc";
            var filter = request.GetFilter();

            var model = new ItemListDTO<TaskListDTO>
            {
                PageSize = request.PageSize,
                TotalCount = await uow.Tasks.CountAsync(),
                PageIndex = request.PageIndex,
                FilteredCount = await uow.Tasks.CountAsync(filter),
                Items = await uow.Tasks.GetDTOAsync(
                    TaskListDTO.Selector,
                    filter,
                    orderBy: x => x.OrderBy(sort),
                    skip: (request.PageIndex - 1) * request.PageSize,
                    take: request.PageSize
                    )
            };

            model.Items.ForEach(async x => x.PicUrl = await minIoService.GetDownloadUrl(x.PicName, $"Tasks"));

            return Result.Success(model);
        }
    }
}
