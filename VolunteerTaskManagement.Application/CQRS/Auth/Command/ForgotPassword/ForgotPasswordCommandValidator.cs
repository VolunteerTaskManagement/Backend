using FluentValidation;

namespace VolunteerTaskManagement.Application.CQRS.Auth
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            //RuleFor(x => x.Email)
            //     .EmailAddress().WithMessage("فرمت ایمیل صحیح نیست!")
            //     .NotEmpty().NotNull().WithMessage("ایمیل اجباری است!");
        }
    }
}