using MediatR;
using Microsoft.AspNetCore.Http;
using Base.Application.Contracts;
using VolunteerTaskManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Base.Application.Contracts.DTOs.Common;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities.State;

namespace VolunteerTaskManagement.Application.CQRS.Tasks
{
    public class TaskCreateCommand : IRequest<Result>
    {
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

        public string? Address { get; set; }

        public DateTime StartDate { get; set; }

        [Display(Name = "تعداد افراد مورد نیاز")]
        public int Count { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public double Lat { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public double Lng { get; set; }
    }

    public class TaskCreateCommandCommand(IVolunteerTaskManagementUnitOfWork uow, IMinIoService minIoService, IJwtManager jwtManager)
        : IRequestHandler<TaskCreateCommand, Result>
    {
        public async Task<Result> Handle(TaskCreateCommand request, CancellationToken cancellationToken)
        {
            var userId = jwtManager.GetUserId();

            var user = await uow.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if(user == null) Result.NotFound("کاربر یافت نشد!");

            if (user!.PhoneNumber == null || user.NationalCode == null)
                return Result.Failure("پروفایل خود را ابتدا تکمیل کنید!");

            var task = new Domain.Entities.VolunteerTask()
            {
                Count = request.Count,
                Title = request.Title,
                Skills = request.Skills,
                PicName = request.Pic?.FileName,
                Description = request.Description,
                NeighborhoodId = request.NeighborhoodId,
                Address = request.Address,
                StartDate = request.StartDate,
                Status = VolunteerTaskStatus.Registered,
                Lat = request.Lat,
                Lng = request.Lng,
            };
           

            if (request.Pic != null)
            {
                var isSuccess = await minIoService.UploadFile(request.Pic, "Tasks/");
                if (!isSuccess)
                    return Result.Failure("عکس آپلود نشد. مجددا تلاش کنید");
            }
            task.State = new RegisterdState();

            await uow.Tasks.AddAsync(task);
            await uow.CommitAsync();

            return Result.Success();
        }
    }
}
