using MediatR;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;
using Base.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskDeleteCommand(long id) : IRequest<Result>
    {
        public long Id { get; set; } = id;
    }

    public class TaskDeleteCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<TaskDeleteCommand, Result>
    {
        public async Task<Result> Handle(TaskDeleteCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var task = await uow.Tasks.FirstOrDefaultAsync(x => x.Id == request.Id && x.CreatedBy == userId);

            if (task is null) return Result.NotFound();

            uow.Tasks.DeleteAsync(task);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
