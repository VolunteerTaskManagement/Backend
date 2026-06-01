using Base.Api.Base;
using EduGuide.Application.CQRS.MediaFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduGuide.Gateway.Controllers.FileService
{
    public class MediaFilesController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        [Route("[action]")]
        //[Authorize]
        public async Task<IActionResult> StramImg([FromQuery] StreamImgQuery query)
        {
            var res = await Mediator.Send(query);
            return File(res.Value.Stream, res.Value.ContentType);
        }
    }
}
