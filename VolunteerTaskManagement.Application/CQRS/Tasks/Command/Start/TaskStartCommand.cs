using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks.Command.Confirm
{
    public class TaskStartCommand : IRequest<Result>
    {
        public long Id { get; set; }



    }
    public class TaskStartCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskStartCommand, Result>
    {
        public async Task<Result> Handle(TaskStartCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var task = await uow.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id &&
            x.CreatedBy == userId
            ) ?? throw new Exception("تسک مورد نظر یافت نشد");

            if (task.VolunteerCount < task.Count)
                throw new Exception("ظرفیت تسک پر نشده است");

            task.State.Start(task);

            await uow.CommitAsync();
            return Result.Success();

        }
    }
}
