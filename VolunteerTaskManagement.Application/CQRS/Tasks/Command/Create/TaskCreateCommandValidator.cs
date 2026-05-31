using FluentValidation;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskCreateCommandValidator : AbstractValidator<TaskCreateCommand>
    {
        public TaskCreateCommandValidator()
        {
            RuleFor(x => x.Pic)
                .NotEmpty().WithMessage("فایل اجباری است!");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان اجباری است!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("عنوان اجباری است!");

            RuleFor(x => x.Skills)
                .NotEmpty().WithMessage("مهارت‌ها اجباری است!");

            RuleFor(x => x.NeighborhoodId)
                .GreaterThan(0).WithMessage("محله اجباری است!");

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("تعداد افراد مورد نیاز اجباری است!");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("آدرس اجباری است!");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("تاریخ شروع اجباری است!");
        }
    }
}
