using Base.Api.Base;
using Base.Application.Contracts.DTOs;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VolunteerTaskManagement.Application.CQRS.Neighborhoods;

namespace VolunteerTaskManagement.Api.Controllers
{
    public class NeighborhoodsController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<ActionResult<Result<List<SelectListDTO>>>> Dropdown([FromQuery] NeighborhoodDropdownQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
