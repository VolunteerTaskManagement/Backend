using Base.Application.Contracts.DTOs.Common;
using MediatR;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;

namespace VolunteerTaskManagement.Application.CQRS.NotficationLogs.Command.Create
{
    public class NotficationLogCreateCommand : IRequest<Result>
    {
        public List<long> UsersId { get; set; } = [];
        public string Title { get; set; } = string.Empty;
    }

    public class NotficationLogCreateCommandHandler(IVolunteerTaskManagementUnitOfWork uow)
        : IRequestHandler<NotficationLogCreateCommand, Result>
    {
        public async Task<Result> Handle(NotficationLogCreateCommand request, CancellationToken cancellationToken)
        {
            var notificationLog = new NotificationLog
            {
                Title = request.Title,
                UsersId = request.UsersId,
            };

            await uow.NotificationLogs.AddAsync(notificationLog);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
