
using Base.Application;
using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using LinqKit;
using MediatR;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Enums;


namespace VolunteerTaskManagement.Application.CQRS.Profile
{
    public class ProfileQuery : IRequest<Result<ProfileDto>>
    {
    }

    public class ProfileQueryHandler(
     IGenericRepository<User, IVolunteerTaskManagementContext> repository,
     IJwtManager jwtManager,
     IMinIoService minIoService
 ) : IRequestHandler<ProfileQuery, Result<ProfileDto>>
    {
        public async Task<Result<ProfileDto>> Handle(ProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var result = await repository.Repository.GetOneDTOAsync(
                ProfileDto.Selector,
                x => x.Id == userId
            ) ?? throw new Exception("کاربر یافت نشد!");


            if (!string.IsNullOrEmpty(result.PicName))
            {
                result.PicUrl = await minIoService.GetDownloadUrl(
                    result.PicName,
                    $"User/Profile/{jwtManager.GetUserName()}/"
                );
            }

            return Result<ProfileDto>.Success(result);
        }
    }

}