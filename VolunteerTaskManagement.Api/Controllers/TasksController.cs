using Base.Api.Base;
using Base.Application;
using Base.Application.Contracts.DTOs.Common;
using Base.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerTaskManagement.Application.CQRS.Tasks;
using VolunteerTaskManagement.Domain.Enums;

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
        public async Task<ActionResult<Result<ItemListDTO<TaskListDTO>>>> GetMyList([FromQuery] TaskGetMyListQuery command)
            => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<ActionResult<Result<ItemListDTO<TaskListDTO>>>> GetList([FromQuery] TaskGetListQuery command)
            => Ok(await Mediator.Send(command));

        [HttpGet("{id:long}")]
        [Authorize(Roles = "Coordinator")]
        public async Task<ActionResult<Result<TaskGetByIdDTO>>> GetById(long id)
            => Ok(await Mediator.Send(new TaskGetByIdQuery(id)));

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Coordinator")]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<ActionResult<Result>> Delete(long id)
            => Ok(await Mediator.Send(new TaskDeleteCommand(id)));

        [HttpGet("skills")]
        public ActionResult<Result> GetSkills(string search)
        {
            var result = EnumExtensions.ToKeyValueList<Skill>(search);

            return Ok(Result.Success(result));
        }
    }
}
