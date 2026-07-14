using MediatR;
using Base.Application.Contracts;
using Microsoft.AspNetCore.Identity;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class ChangeForgotPasswordCommand : IRequest<Result<bool>>
    {
        public string UserName { get; set; }
        //public long Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmedNewPassword { get; set; }
    }

    public class ChangeForgotPasswordCommandHandler(/*IRedisService redisService,*/ IGenericRepository<User, IVolunteerTaskManagementContext> uow)
        : IRequestHandler<ChangeForgotPasswordCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ChangeForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            //var code = await redisService.GetVerificationCodeAsync(request.Email);
            //if (string.IsNullOrEmpty(code) || long.Parse(code) != request.Code)
            //    throw new InvalidOperationException("کد منقضی یا نامتعبر است!");

            //var isConfirmed = await redisService.GetVerificationCodeAsync(request.Email);
            //if (isConfirmed != "1")
            //    throw new Exception("شما اجازه تغییر رمزعبور را ندارید!");

            var user = await uow.Repository.FirstOrDefaultAsync(x => x.UserName.Equals(request.UserName))
                ?? throw new Exception("کاربری با این نام کاربری وجود ندارد!");

            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.NewPassword);

            //await redisService.RemoveVerificationCodeAsync(request.UserName);

            await uow.CommitAsync();
            return Result<bool>.Success(true);
        }
    }
}