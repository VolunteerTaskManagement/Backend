using Base.Api.Base;
using Base.Application;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using Base.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerTaskManagement.Api.Hubs;
using VolunteerTaskManagement.Application.CQRS.NotficationLogs.Command.Create;
using VolunteerTaskManagement.Application.CQRS.Tasks;
using VolunteerTaskManagement.Application.CQRS.Tasks.Command.Confirm;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Api.Controllers
{
    public class TasksController(IMediator mediator, NotificationHub notificationHub, IJwtManager jwtManager) : BaseApiController(mediator)
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
        [Authorize]
        public async Task<ActionResult<Result<ItemListDTO<TaskListDTO>>>> GetMyList([FromQuery] TaskGetMyListQuery command)
            => Ok(await Mediator.Send(command));

        [HttpGet]
        public async Task<ActionResult<Result<ItemListDTO<TaskListDTO>>>> GetList([FromQuery] TaskGetListQuery command)
            => Ok(await Mediator.Send(command));

        [HttpGet("{id:long}")]
        [Authorize]
        public async Task<ActionResult<Result<TaskGetByIdDTO>>> GetById(long id)
            => Ok(await Mediator.Send(new TaskGetByIdQuery(id)));

        [HttpGet("{id:long}/volunteer-confirmations")]
        [Authorize(Roles = "Coordinator")]
        public async Task<ActionResult<Result<List<TaskGetVolunteerConfirmationsDTO>>>> GetVolunteerConfirmations(long id)
            => Ok(await Mediator.Send(new TaskGetVolunteerConfirmationsQuery(id)));

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Coordinator")]
        [ProducesResponseType(typeof(Result), 200)]
        public async Task<ActionResult<Result>> Delete(long id)
        {
            var res = await Mediator.Send(new TaskDeleteCommand(id));
            return Ok(res);
        }

        [HttpGet("skills")]
        public ActionResult<Result> GetSkills(string search)
        {
            var result = EnumExtensions.ToKeyValueList<Skill>(search);

            return Ok(Result.Success(result));
        }

        [HttpPost]
        [Authorize(Roles = "Volunteer")]
        [Route("assign")]
        public async Task<ActionResult<Result>> Assign([FromBody] TaskAssignCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [Route("complete-by-volunteer")]
        [Authorize(Roles = "Volunteer")]
        public async Task<ActionResult<Result<List<string?>>>> CompleteByVolunteer([FromBody] TaskCompleteByVolunteerCommand command)
        {
            var res = await Mediator.Send(command);

            if (res.IsSuccess)
            {
                var userName = jwtManager.GetName();

                var data = res.Value;

                if (data != null && data.Count >= 2)
                {
                    var taskTitle = data[0];
                    var createdByStr = data[1] ?? "0";

                    if (long.TryParse(createdByStr, out var createdBy))
                    {
                        await Mediator.Send(new NotficationLogCreateCommand()
                        {
                            UsersId = [createdBy],
                            Title = taskTitle
                        });

                        notificationHub?.SendNotification(
                            $"تسک {taskTitle} توسط {userName} تکمیل شد.",
                            [createdBy]
                        );
                    }
                }
            }

            return Ok(res);
        }

        [HttpPost]
        [Authorize(Roles = "Volunteer")]
        [Route("unassign")]
        public async Task<ActionResult<Result>> Unassign([FromBody] TaskUnassignCommand command)
            => Ok(await Mediator.Send(command));

        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        [Route("start")]
        public async Task<ActionResult<Result<string>>> Start([FromBody] TaskStartCommand command)
        {
            var res = await Mediator.Send(command);

            if (res.IsSuccess)
            {
                var volunteersId = await Mediator.Send(new TaskGetVolunteersQuery(command.Id)) ?? [];
                if (volunteersId.Count > 0)
                {
                    await Mediator.Send(new NotficationLogCreateCommand()
                    {
                        UsersId = volunteersId,
                        Title = res.Value
                    });
                    notificationHub?.SendNotification($"تسک {res.Value} شروع شد.", volunteersId);
                }
            }

            return Ok(res);
        }


        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        [Route("confirm")]
        public async Task<ActionResult<Result<string>>> Confirm([FromBody] TaskConfirmCommand command)
        {
            var res = await Mediator.Send(command);

            if (res.IsSuccess)
            {
                var volunteersId = await Mediator.Send(new TaskGetVolunteersQuery(command.Id)) ?? [];
                if (volunteersId.Count > 0)
                {
                    await Mediator.Send(new NotficationLogCreateCommand()
                    {
                        UsersId = volunteersId,
                        Title = res.Value
                    });
                    notificationHub?.SendNotification($"تسک {res.Value} تایید شد.", volunteersId);
                }
            }

            return Ok(res);
        }




        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        [Route("cancel")]
        public async Task<ActionResult<Result>> Cancel([FromBody] TaskCancelCommand command)
        {
            var res = await Mediator.Send(command);

            if (res.IsSuccess)
            {
                var volunteersId = await Mediator.Send(new TaskGetVolunteersQuery(command.Id)) ?? [];
                if (volunteersId.Count > 0)
                {
                    await Mediator.Send(new NotficationLogCreateCommand()
                    {
                        UsersId = volunteersId,
                        Title = res.Value
                    });
                    notificationHub?.SendNotification($"تسک {res.Value} لغو شد.", volunteersId);
                }
            }

            return Ok(res);
        }


    }
}
