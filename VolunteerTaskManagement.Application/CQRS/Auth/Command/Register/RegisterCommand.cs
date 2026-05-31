using MediatR;
using Microsoft.AspNetCore.Identity;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Enums;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class RegisterCommand : IRequest<Result<bool>>
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Role Role { get; set; }
        public string? Password { get; set; }
        public string? ConfirmedPassword { get; set; }
    }

    public class RegisterCommandHandler(IVolunteerTaskManagementUnitOfWork uow)
        : IRequestHandler<RegisterCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role == Role.Coordinator ? "Coordinator" : "Volunteer",
                UserName = request.UserName,
            };
            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, request.Password!);

            await uow.Users.AddAsync(user);
            await uow.CommitAsync();

            return Result.Success(true);
        }
    }
}