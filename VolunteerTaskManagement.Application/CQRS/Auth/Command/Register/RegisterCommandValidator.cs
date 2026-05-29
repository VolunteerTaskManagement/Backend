using Base.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;
using FluentValidation;
using System.Text.RegularExpressions;
using VolunteerTaskManagement.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator(IGenericRepository<User, IVolunteerTaskManagementContext> genericRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("نام کاربری اجباری است!")
                    .Must(userName =>
                        !genericRepository.Repository.Any(u => u.UserName == userName))
                    .WithMessage("این نام کاربری قبلاً ثبت شده است!");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("نام اجباری است!")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("!نام باید فقط شامل حروف فارسی باشد");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("نام خانوادگی اجباری است!")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام خانوادگی باید فقط شامل حروف فارسی باشد!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است.")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل ۸ کاراکتر باشد!")
                .Must(BeAValidPassword)
                .WithMessage("رمز عبور باید شامل حداقل یک نماد، یک حرف بزرگ و یک عدد باشد!");

            RuleFor(x => x.ConfirmedPassword)
                .NotEmpty().WithMessage("تأیید رمز عبور الزامی است.")
                .Equal(x => x.Password).WithMessage("تأیید رمز عبور باید با رمز عبور مطابقت داشته باشد!");
        }

        private bool BeAValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            var hasSymbol = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(password);
            var hasUpper = new Regex("[A-Z]").IsMatch(password);
            var hasNumber = new Regex("[0-9]").IsMatch(password);

            return hasSymbol && hasUpper && hasNumber;
        }
    }
}