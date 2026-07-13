using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.NotficationLogs.Query.GetCount
{
    public class NotificationLogGetCountQuery : IRequest<Result>
    {
    }

    public class NotificationLogGetCountQueryHandler(IVolunteerTaskManagementUnitOfWork uow, IJwtManager jwtManager)
        : IRequestHandler<NotificationLogGetCountQuery, Result>
    {
        public async Task<Result> Handle(NotificationLogGetCountQuery request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var unSeenNotifCount = await uow.NotificationLogs.CountAsync(x => !x.IsSeen && x.UsersId.Contains(userId.Value));

            return Result.Success(unSeenNotifCount);
        }
    }
}
