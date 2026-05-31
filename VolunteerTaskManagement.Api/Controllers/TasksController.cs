using Base.Api.Base;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerTaskManagement.Application.CQRS.Tasks;

namespace VolunteerTaskManagement.Api.Controllers
{
    public class TasksController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        public async Task<ActionResult<Result>> Create([FromForm] TaskCreateCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPut]
        [Authorize(Roles = "Coordinator")]
        public async Task<ActionResult<Result>> Update([FromForm] TaskUpdateCommand command)
            => Ok(await Mediator.Send(command));

        [HttpGet]
        [Route("my")]
        [Authorize(Roles = "Coordinator")]
        public async Task<ActionResult<Result>> GetMyList([FromQuery] TaskGetMyListQuery command)
            => Ok(await Mediator.Send(command));
    }
}
