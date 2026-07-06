using MediatR;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskGetVolunteerConfirmationsQuery(long id)
        : IRequest<Result<List<TaskGetVolunteerConfirmationsDTO>>>
    {
        public long TaskId { get; set; } = id;
    }

    public class TaskGetVolunteerConfirmationsQueryHandler(IJwtManager jwtManager, IVolunteerTaskManagementUnitOfWork uow)
        : IRequestHandler<TaskGetVolunteerConfirmationsQuery, Result<List<TaskGetVolunteerConfirmationsDTO>>>
    {
        public async Task<Result<List<TaskGetVolunteerConfirmationsDTO>>> Handle(TaskGetVolunteerConfirmationsQuery request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var model = await uow.UserTasks.GetDTOAsync(
                TaskGetVolunteerConfirmationsDTO.Selector,
                x => x.Task.CreatedBy == userId && x.TaskId == request.TaskId
                );

            return Result.Success(model);
        }
    }
}
