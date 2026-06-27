using MediatR;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskGetByIdQuery(long id) : IRequest<Result<TaskGetByIdDTO>>
    {
        public long Id { get; set; } = id;
    }

    public class TaskGetByIdQueryHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager, IMinIoService minIoService)
        : IRequestHandler<TaskGetByIdQuery, Result<TaskGetByIdDTO>>
    {
        public async Task<Result<TaskGetByIdDTO>> Handle(TaskGetByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();
            var role = jwtManager.GetRole();

            var task = await uow.Tasks.GetOneDTOAsync(
                TaskGetByIdDTO.Selector(userId!.Value, role),
                x => x.CreatedBy == userId && x.Id == request.Id);
            
            if (task is null) return Result.NotFound<TaskGetByIdDTO>("رکورد موردنظر یافت نشد");

            var coordinator = await uow.Users.FirstOrDefaultAsync(x => x.Id == task.CreatorId);

            task.CoordinatorName = coordinator?.FirstName + " " + coordinator?.LastName;
            task.Mobile = coordinator?.PhoneNumber;
            task.PicUrl = await minIoService.GetDownloadUrl(task.PicName, "Tasks");

            return Result.Success(task);
        }
    }
}
