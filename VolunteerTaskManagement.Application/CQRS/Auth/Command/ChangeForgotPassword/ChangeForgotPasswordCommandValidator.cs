using FluentValidation;
using System.Text.RegularExpressions;

namespace VolunteerTaskManagement.Application.CQRS.Auth.Command.ChangeForgotPassword
{
    public class ChangeForgotPasswordCommandValidator : AbstractValidator<ChangeForgotPasswordCommand>
    {
        public ChangeForgotPasswordCommandValidator()
        {
            //RuleFor(x => x.Email)
            //    .EmailAddress().WithMessage("فرمت ایمیل درست نمیباشد!")
            //    .NotEmpty().NotNull().WithMessage("ایمیل اجباری است!");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز عبور الزامی است.")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل ۸ کاراکتر باشد.")
                .Must(BeAValidPassword)
                .WithMessage("رمز عبور باید شامل حداقل یک نماد، یک حرف بزرگ و یک عدد باشد.");

            RuleFor(x => x.ConfirmedNewPassword)
                .NotEmpty().WithMessage("تأیید رمز عبور الزامی است.")
                .Equal(x => x.NewPassword).WithMessage("تأیید رمز عبور باید با رمز عبور مطابقت داشته باشد.");
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