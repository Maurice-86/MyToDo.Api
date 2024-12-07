using System.ComponentModel.DataAnnotations;

namespace MyToDo.Api.Domain.Entities
{
    public class ToDo : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Content { get; set; }

        [Range(0, 1)]
        public int Status { get; set; } // 0： 待办，1：已完成
    }
}
