using Base.Application.Contracts;
using FluentValidation;

namespace VolunteerTaskManagement.Application.Profile.Command.Update
{
    public class ProfileUpdateCommandValidator : AbstractValidator<ProfileUpdateCommand>
    {
        public ProfileUpdateCommandValidator(IJwtManager jwtManager)
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage("نام اجباری است!")
                 .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("!نام باید فقط شامل حروف فارسی باشد");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("نام خانوادگی اجباری است!")
                .Matches(@"^[\u0600-\u06FF\s]+$").WithMessage("نام خانوادگی باید فقط شامل حروف فارسی باشد!");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("شماره تلفن اجباری است.")
                .Matches(@"^09\d{9}$").WithMessage("شماره تلفن نامعتبر است");

            When(x => jwtManager.GetRole() == "Volunteer", () =>
            {
                RuleFor(x => x.BirthDate)
                    .NotEmpty().WithMessage("تاریخ تولد الزامی است!")
                    .Must(date => date.Value.Date <= DateTime.Today.AddYears(-10))
                    .WithMessage("سن باید حداقل ۱۰ سال باشد!")
                    .Must(date => date.Value.Date >= DateTime.Today.AddYears(-100))
                    .WithMessage("سن نباید بیشتر از ۱۰۰ سال باشد!");

                RuleFor(x => x.NeighborhoodId)
                    .NotEmpty().WithMessage("محله الزامی است");

                RuleFor(x => x.Skills)
                    .NotEmpty().WithMessage("مهارت‌ها اجباری است!");
            });

            RuleFor(x => x.NationalCode)
                .Length(10)
                .WithMessage("کد ملی باید ۱۰ رقم باشد.")
                .Matches(@"^\d{10}$")
                .WithMessage("کد ملی باید فقط شامل عدد باشد.")
                .When(x => jwtManager.GetRole() == "Coordinator");


        }
    }
}