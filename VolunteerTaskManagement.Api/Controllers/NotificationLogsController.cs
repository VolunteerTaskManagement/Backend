using Base.Api.Base;
using Base.Application;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerTaskManagement.Application.CQRS.NotficationLogs.Query.GetList;

namespace VolunteerTaskManagement.Api.Controllers
{
    public class NotificationLogsController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Result<List<NotificationLogListDTO>>>> Get([FromQuery] NotificationLogGetListQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
