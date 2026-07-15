using MediatR;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskGetVolunteersQuery(long taskId) : IRequest<List<long>>
    {
        public long TaskId { get; set; } = taskId;
    }

    public class TaskGetVolunteersQueryHandler(IVolunteerTaskManagementUnitOfWork uow)
            : IRequestHandler<TaskGetVolunteersQuery, List<long>>
    {
        public async Task<List<long>> Handle(TaskGetVolunteersQuery request, CancellationToken cancellationToken)
        {
            var userTasks = await uow.UserTasks.GetAsync(x => x.TaskId == request.TaskId);

            var usersId = userTasks?.Select(x => x.VolunteerId).ToList();

            return usersId ?? [];
        }
    }
}
