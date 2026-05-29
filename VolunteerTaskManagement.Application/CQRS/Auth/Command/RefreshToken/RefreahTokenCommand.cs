using Base.Application.Contracts;
using Base.Application.Contracts.DTOs;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Domain.Entities;
using MediatR;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class RefreshTokenCommand : IRequest<Result<TokenResponseDto>>
    {
        //public long UserId { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler(IJwtManager jwtManager, IGenericRepository<User, IVolunteerTaskManagementContext> uow)
        : IRequestHandler<RefreshTokenCommand, Result<TokenResponseDto>>
    {
        public async Task<Result<TokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();
            var user = await uow.Repository.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new Exception("شناسه کاربری نامعتبر است!");

            var userDto = new UserDTO()
            {
                UserName = user.UserName,
                Id = user.Id,
                Role = user.Role,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime.Value
            };
            var tokenResponseDto = jwtManager.RefreshTokensAsync(userDto, request.RefreshToken);

            user.RefreshToken = tokenResponseDto.RefreshToken;
            await uow.CommitAsync();

            return Result<TokenResponseDto>.Success(tokenResponseDto);
        }
    }
}