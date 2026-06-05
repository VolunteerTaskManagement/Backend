using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks.Command.Confirm
{
    public class TaskConfirmCommand : IRequest<Result>
    {
        public long Id { get; set; }



    }
    public class TaskConfirmCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskConfirmCommand, Result>
    {
        public async Task<Result> Handle(TaskConfirmCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var task = await uow.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id &&
            x.CreatedBy == userId
            ) ?? throw new Exception("تسک مورد نظر یافت نشد");
            task.State.Confirm(task);

            await uow.CommitAsync();
            return Result.Success();

        }
    }
}
