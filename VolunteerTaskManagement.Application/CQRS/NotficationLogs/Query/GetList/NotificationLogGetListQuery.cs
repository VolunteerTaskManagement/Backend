using Base.Application;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.NotficationLogs.Query.GetList
{
    public class NotificationLogGetListQuery : IRequest<Result<List<NotificationLogListDTO>>>
    {
    }

    public class NotificationLogGetListQueryHandler(IJwtManager jwtManager, IVolunteerTaskManagementUnitOfWork uow)
        : IRequestHandler<NotificationLogGetListQuery, Result<List<NotificationLogListDTO>>>
    {
        public async Task<Result<List<NotificationLogListDTO>>> Handle(NotificationLogGetListQuery request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();
            //var sort = "id desc";

            var notificationLogs = await uow.NotificationLogs.GetAsync(x => !x.IsSeen && x.UsersId.Contains(userId.Value)) ?? [];

            var model = new List<NotificationLogListDTO>();
            foreach (var notificationLog in notificationLogs.OrderByDescending(n => n.Id))
            {
                model.Add(new NotificationLogListDTO
                {
                    Id = notificationLog.Id,
                    UsersId = notificationLog.UsersId,
                    Title = notificationLog.Title,
                    IsSeen = notificationLog.IsSeen
                });
                notificationLog.IsSeen = true;
            }

            await uow.CommitAsync();

            //var model = await uow.NotificationLogs.GetDTOAsync(
            //        NotificationLogListDTO.Selector,
            //        x => !x.IsSeen && x.UsersId.Contains(userId.Value),
            //        orderBy: x => x.OrderBy(sort)
            //        );

            return Result.Success(model);
        }
    }
}
