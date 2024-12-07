using System.ComponentModel.DataAnnotations;

namespace MyToDo.Api.Domain.Entities
{
    public class Memo : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Content { get; set; }
    }
}
