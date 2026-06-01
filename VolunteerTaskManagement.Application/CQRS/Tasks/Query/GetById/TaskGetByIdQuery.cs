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

            var task = await uow.Tasks.GetOneDTOAsync(
                TaskGetByIdDTO.Selector,
                x => x.CreatedBy == userId && x.Id == request.Id);
            
            if (task is null) return Result.NotFound<TaskGetByIdDTO>("رکورد موردنظر یافت نشد");

            task.PicUrl = await minIoService.GetDownloadUrl(task.PicName, "Tasks");

            return Result.Success(task);
        }
    }
}
