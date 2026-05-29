using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using Base.Application.Exceptions;
using VolunteerTaskManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class ChangePasswordCommand
        : IRequest<Result>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedNewPassword { get; set; }
    }
    public class ChangePasswordCommandHandler(IGenericRepository<User, IVolunteerTaskManagementContext> genericRepository, IJwtManager jwtManager)
        : IRequestHandler<ChangePasswordCommand, Result>
    {
        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await genericRepository.Repository.GetByIdAsync(jwtManager.GetUserId().Value);
            if (user == null ||
                new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.OldPassword) == PasswordVerificationResult.Failed)
                throw new LoginException("رمزعبور فعلی اشتباه است!");

            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.NewPassword);

            await genericRepository.CommitAsync();
            return Result<bool>.Success(true);
        }
    }
}
