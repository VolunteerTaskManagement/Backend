using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks.Command.Confirm
{
    public class TaskConfirmCommand : IRequest<Result<string>>
    {
        public long Id { get; set; }



    }
    public class TaskConfirmCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskConfirmCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(TaskConfirmCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var task = await uow.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id &&
            x.CreatedBy == userId,
            includes: x => x.Include(x => x.UserTasks)
            ) ?? throw new Exception("تسک مورد نظر یافت نشد");

            if (task.UserTasks.Any(x => x.IsCompleted == false))
                throw new Exception("این تسک توسط تمام داوطلبان اتمام کار نخورده است!");

            task.State.Confirm(task);

            await uow.CommitAsync();
            return Result<string>.Success(task.Title);

        }
    }
}
