namespace MyToDo.Api.Context
{
    public class Memo : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
    }
}
