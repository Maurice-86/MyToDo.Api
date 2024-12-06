namespace MyToDo.Api.Context
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? NickName { get; set; }
    }
}
