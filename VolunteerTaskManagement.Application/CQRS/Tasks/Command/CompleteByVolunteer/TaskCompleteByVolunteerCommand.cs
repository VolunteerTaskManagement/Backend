using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskCompleteByVolunteerCommand(long id) : IRequest<Result>
    {
        public long Id { get; set; } = id;
    }

    public class TaskCompleteByVolunteerCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskCompleteByVolunteerCommand, Result>
    {
        public async Task<Result> Handle(TaskCompleteByVolunteerCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var userTask = await uow.UserTasks.FirstOrDefaultAsync(x => x.TaskId == request.Id && x.VolunteerId == userId)
                ?? throw new Exception("تسک مورد نظر یافت نشد!");

            userTask.IsCompleted = true;

            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
