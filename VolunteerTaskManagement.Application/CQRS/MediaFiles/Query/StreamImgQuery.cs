using Base.Application.Contracts.DTOs.Common;
using MediatR;

namespace EduGuide.Application.CQRS.MediaFiles
{
    public class StreamImgQuery : IRequest<Result<StreamImgDTO>>
    {
        public string FileUrl { get; set; }
    }

    public class StreamImgQueryHandler(IHttpClientFactory httpClientFactory) : IRequestHandler<StreamImgQuery, Result<StreamImgDTO>>
    {
        public async Task<Result<StreamImgDTO>> Handle(StreamImgQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var response = await httpClient.GetAsync(request.FileUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType;

            return Result<StreamImgDTO>.Success(new StreamImgDTO(stream, contentType));
        }
    }
}