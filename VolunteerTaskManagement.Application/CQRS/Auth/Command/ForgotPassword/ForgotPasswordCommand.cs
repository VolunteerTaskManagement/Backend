using Base.Application.Contracts;
using Base.Application.Contracts.DTOs.Common;
using MediatR;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class ForgotPasswordCommand : IRequest<Result<bool>>
    {
        public string UserName { get; set; }
    }

    public class ForgotPasswordCommandHandler(IEmailService emailService, IRedisService redisService)
        : IRequestHandler<ForgotPasswordCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var verificationCode = new Random().Next(100000, 999999).ToString();
            //await redisService.SetVerificationCodeAsync(request.Email, verificationCode, TimeSpan.FromMinutes(5));

            //emailService.SendEmail(verificationCode, "Verification Code", request.Email);

            return Result<bool>.Success(true);
        }
    }
}