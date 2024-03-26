using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class NewQuizDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }
    }
}