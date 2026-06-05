using MediatR;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskUnassignCommand(long id) : IRequest<Result>
    {
        public long Id { get; set; } = id;
    }

    public class TaskUnassignCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskUnassignCommand, Result>
    {
        public async Task<Result> Handle(TaskUnassignCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var userTask = await uow.UserTasks.FirstOrDefaultAsync(
                x => x.TaskId == request.Id && x.VolunteerId == userId,
                includes: x => x.Include(x => x.Task));
            if (userTask == null)
                return Result.NotFound("شما در این تسک مشارکت نکرده‌اید!");

            if (userTask.Task.Status != Domain.Enums.VolunteerTaskStatus.Registered)
                return Result.Failure("لفو ثبت نام در این مرحله امکان پذیر نیست!");

            userTask.Task.Count += 1;

            uow.UserTasks.DeleteAsync(userTask);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
