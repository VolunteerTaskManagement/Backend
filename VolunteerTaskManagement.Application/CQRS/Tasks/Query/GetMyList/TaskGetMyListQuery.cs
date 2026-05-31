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
    public class TaskGetMyListQuery : IRequest<Result<ItemListDTO<TaskListDTO>>>
    {
        public List<long> NeighborhoodIds { get; set; } = [];
        public List<Skill> Skills { get; set; } = [];
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public Expression<Func<VolunteerTask, bool>> GetFilter()
        {
            var filter = PredicateBuilder.New<VolunteerTask>(true);

            if (NeighborhoodIds.Count != 0)
                filter.And(x => NeighborhoodIds.Contains(x.Id));

            if (Skills.Count != 0)
                filter.And(x => x.Skills.Any(s => Skills.Contains(s)));
            // TODO: Filter States

            return filter;
        }
    }

    public class TaskGetMyListQueryHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager, IMinIoService minIoService)
        : IRequestHandler<TaskGetMyListQuery, Result<ItemListDTO<TaskListDTO>>>
    {
        public async Task<Result<ItemListDTO<TaskListDTO>>> Handle(TaskGetMyListQuery request, CancellationToken cancellationToken)
        {
            var sort = "id desc";
            var userId = jwtManager.GetUserId();
            var role = jwtManager.GetRole();
            var filter = request.GetFilter();

            if (role == "Coordinator")
                filter = filter.And(x => x.CreatedBy == userId);
            else if (role == "Volunteer")
                filter = filter.And(x => x.UserTasks.Any(u => u.CreatedBy == userId)); // تست شود

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
