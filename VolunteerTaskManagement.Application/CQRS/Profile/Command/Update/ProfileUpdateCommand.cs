using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Enums;

namespace VolunteerTaskManagement.Application.Profile.Command.Update
{
    public class ProfileUpdateCommand : IRequest<Result<bool>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public long NeighborhoodId { get; set; }

        public IFormFile? ProfilePic { get; set; }
        public List<Skill> Skills { get; set; } = [];

    }
    public class ProfileUpdateCommandHandler(
    IGenericRepository<User, IVolunteerTaskManagementContext> repository,
    IJwtManager jwtManager,
    IMinIoService minIoService
) : IRequestHandler<ProfileUpdateCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ProfileUpdateCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var user = await repository.Repository
                .FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new Exception ("کاربر یافت نشد!");

           
            user.Skills=request.Skills;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.BirthDate = request.BirthDate;
            user.NeighborhoodId = request.NeighborhoodId;

            if (request.ProfilePic != null)
            {
                await minIoService.UploadFile(
                    request.ProfilePic,
                    $"User/Profile/{jwtManager.GetUserName()}/"
                );

                user.PicName = request.ProfilePic.FileName;
            }

            await repository.CommitAsync();

            return Result<bool>.Success(true);
        }
    }

}