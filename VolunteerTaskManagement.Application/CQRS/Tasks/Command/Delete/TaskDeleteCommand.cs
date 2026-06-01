using MediatR;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskDeleteCommand(long id) : IRequest<Result>
    {
        public long Id { get; set; } = id;
    }

    public class TaskDeleteCommandHandler(IVolunteerTaskManagementUnitOfWork uow)
        : IRequestHandler<TaskDeleteCommand, Result>
    {
        public async Task<Result> Handle(TaskDeleteCommand request, CancellationToken cancellationToken)
        {
            var task = await uow.Tasks.GetByIdAsync(request.Id);

            if (task is null) return Result.NotFound();

            uow.Tasks.DeleteAsync(task);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
