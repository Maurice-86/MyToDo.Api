using System.ComponentModel.DataAnnotations;

namespace MyToDo.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }

        [Required]
        [Length(3, 20)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
    }
}
