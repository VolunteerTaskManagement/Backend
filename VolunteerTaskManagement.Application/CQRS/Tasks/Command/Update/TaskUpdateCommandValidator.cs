using FluentValidation;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskUpdateCommandValidator : AbstractValidator<TaskUpdateCommand>
    {
        public TaskUpdateCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان اجباری است!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("عنوان اجباری است!");

            RuleFor(x => x.Skills)
                .NotEmpty().WithMessage("مهارت‌ها اجباری است!");

            RuleFor(x => x.NeighborhoodId)
                .GreaterThan(0).WithMessage("محله اجباری است!");

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("شناسه اجباری است!");

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("تعداد افراد مورد نیاز اجباری است!");
        }
    }
}
