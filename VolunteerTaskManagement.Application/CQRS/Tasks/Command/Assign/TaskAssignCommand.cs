using MediatR;
using Base.Application.Contracts;
using Microsoft.EntityFrameworkCore;
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

            var task = await uow.Tasks.FirstOrDefaultAsync(
                x => x.Id == request.Id,
                includes: x => x.Include(x => x.UserTasks)
                );

            if (task == null)
                return Result.NotFound("تسک مورد نظر یافت نشد!");

            if (task.UserTasks?.Any(x => x.CreatedBy == user.Id) ?? false)
                return Result.Failure("شماره در این تسک ثبت نام کرده‌اید!");

            if (task.Status != Domain.Enums.VolunteerTaskStatus.Registered)
                return Result.Failure("ثبت نام در این مرحله امکان پذیر نیست!");

            if (task.VolunteerCount >= task.Count)
                return Result.Failure("ظرفیت تسک به حد نصاب رسیده است!");

            task.VolunteerCount += 1;

            var userTask = new UserTask
            {
                VolunteerId = userId!.Value,
                TaskId = request.Id,
            };

            await uow.UserTasks.AddAsync(userTask);

            try
            {
                await uow.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result.Failure("اطلاعات این تسک همزمان توسط کاربر دیگری تغییر کرده است. لطفاً مجدداً تلاش کنید.");
            }

            return Result.Success();
        }
    }
}