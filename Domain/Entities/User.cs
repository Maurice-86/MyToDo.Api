using System.ComponentModel.DataAnnotations;

namespace MyToDo.Api.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [Length(3, 20)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
