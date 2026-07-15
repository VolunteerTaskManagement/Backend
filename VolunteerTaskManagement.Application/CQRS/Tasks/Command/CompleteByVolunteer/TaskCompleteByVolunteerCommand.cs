using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskCompleteByVolunteerCommand(long id) : IRequest<Result<List<string?>>>
    {
        public long Id { get; set; } = id;
    }

    public class TaskCompleteByVolunteerCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskCompleteByVolunteerCommand, Result<List<string?>>>
    {
        public async Task<Result<List<string?>>> Handle(TaskCompleteByVolunteerCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var userTask = await uow.UserTasks.FirstOrDefaultAsync(
                x => x.TaskId == request.Id && x.VolunteerId == userId,
                includes: x => x.Include(x => x.Task)) ?? throw new Exception("تسک مورد نظر یافت نشد!");

            if (userTask.Task.Status != Domain.Enums.VolunteerTaskStatus.InProgress)
                throw new Exception("در این مرحله امکان ثبت پایان کار وجود ندارد!");

            if (userTask.IsCompleted)
                return Result.Failure<List<string?>>(new Error("400", "این تسک قبلا تایید شده است!"));

            userTask.IsCompleted = true;

            await uow.CommitAsync();

            return Result.Success<List<string?>>([userTask.Task.Title, userTask?.Task?.CreatedBy?.ToString()]);
        }
    }
}
