using System.ComponentModel.DataAnnotations;

namespace Base.Application.Contracts.DTOs
{
    public record SelectListDTO(
        [property: Display(Name = "شناسه")] long Id,
        [property: Display(Name = "عنوان")] string Title
    );
}
