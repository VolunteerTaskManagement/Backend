using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks.Command.Confirm
{
    public class TaskCancelCommand : IRequest<Result>
    {
        public long Id { get; set; }



    }
    public  class TaskCancelCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskCancelCommand, Result>
    {
        public  async Task<Result> Handle(TaskCancelCommand request, CancellationToken cancellationToken)
        {
            var userId=jwtManager.GetUserId();
           
            var task = await uow.Tasks.FirstOrDefaultAsync(x=>x.Id == request.Id &&
            x.CreatedBy == userId
            )?? throw new Exception("تسک مورد نظر یافت نشد");
            task.State.Cancel(task);

            await uow.CommitAsync();
            return Result.Success();

        }
    }
}
