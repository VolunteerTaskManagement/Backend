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
                .NotEmpty().Matches(@"^09\d{9}$")
                .WithMessage("شماره تلفن نامعتبر است");

            When(x => jwtManager.GetRole() == "Volunteer", () =>
            {
                RuleFor(x => x.BirthDate)
                    .NotEmpty().WithMessage("تاریخ تولد الزامی است");

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