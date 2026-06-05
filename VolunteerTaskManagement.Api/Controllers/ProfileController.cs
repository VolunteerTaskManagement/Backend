using Base.Api.Base;
using Base.Application;
using Base.Application.Contracts.DTOs.Common;
using Base.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerTaskManagement.Application.CQRS.Profile;
using VolunteerTaskManagement.Application.Profile.Command.Update;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Api.Controllers
{
    public class ProfileController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        [Authorize(Roles = "Volunteer")]
        public async Task<ActionResult<Result<ProfileDto>>> Get([FromQuery] ProfileQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPut]
        [Authorize(Roles = "Volunteer")]
        public async Task<ActionResult<Result<bool>>> Update([FromForm] ProfileUpdateCommand command)
        {
            return await Mediator.Send(command);
        }

    }

}
