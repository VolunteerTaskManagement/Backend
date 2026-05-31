using MediatR;
using System.ComponentModel.DataAnnotations;
using Base.Application.Contracts.DTOs.Common;
using Microsoft.AspNetCore.Http;
using VolunteerTaskManagement.Domain.Enums;
using VolunteerTaskManagement.Application.Contracts;
using Base.Application.Contracts;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskUpdateCommand : IRequest<Result>
    {
        [Display(Name = "شناسه")]
        public long Id { get; set; }

        [Display(Name = "تصویر")]
        public IFormFile? Pic { get; set; }

        [Display(Name = "عنوان")]
        public string? Title { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "مهارت‌ها")]
        public List<Skill> Skills { get; set; } = [];

        [Display(Name = "محله")]
        public long NeighborhoodId { get; set; }

        [Display(Name = "تعداد افراد مورد نیاز")]
        public int Count { get; set; }

        public string? Address { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class TaskUpdateCommandHandler(IVolunteerTaskManagementUnitOfWork uow, IMinIoService minIoService)
        : IRequestHandler<TaskUpdateCommand, Result>
    {
        public async Task<Result> Handle(TaskUpdateCommand request, CancellationToken cancellationToken)
        {
            var task = await uow.Tasks.GetByIdAsync(request.Id)
                ?? throw new Exception("تسک مورد نظر یافت نشد!");

            task.Count = request.Count;
            task.Title = request.Title;
            task.Skills = request.Skills;
            task.Description = request.Description;
            task.NeighborhoodId = request.NeighborhoodId;
            task.Address = request.Address;
            task.StartDate = request.StartDate;

            if (request.Pic != null)
            {
                var isSuccess = await minIoService.UploadFile(request.Pic, "Tasks/");
                if (!isSuccess)
                    return Result.Failure("عکس آپلود نشد. مجددا تلاش کنید");

                task.PicName = request.Pic.FileName;
            }

            uow.Tasks.UpdateAsync(task);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
