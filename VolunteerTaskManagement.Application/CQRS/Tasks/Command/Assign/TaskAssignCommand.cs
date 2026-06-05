using MediatR;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskAssignCommand(long id) : IRequest<Result>
    {
        public long Id { get; set; } = id;
    }

    public class TaskAssignCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskAssignCommand, Result>
    {
        public async Task<Result> Handle(TaskAssignCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var user = await uow.Users.GetByIdAsync(userId)
                ?? throw new Exception("کاربر یافت نشد!");

            if (!user.NeighborhoodId.HasValue || user.BirthDate == null || user.PhoneNumber == null || user.Skills == null)
                throw new Exception("پروفایل خود را تکمیل نمایید!");

            var task = await uow.Tasks.GetByIdAsync(request.Id);
            if (task == null)
                return Result.NotFound("تسک مورد نظر یافت نشد!");

            if (task.Status != Domain.Enums.VolunteerTaskStatus.Registered)
                return Result.Failure("ثبت نام در این مرحله امکان پذیر نیست!");

            task.Count -= 1;

            var userTask = new UserTask
            {
                VolunteerId = userId!.Value,
                TaskId = request.Id,
            };

            await uow.UserTasks.AddAsync(userTask);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
