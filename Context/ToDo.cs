namespace MyToDo.Api.Context
{
    public class ToDo : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public int Status { get; set; } // 0： 待办，1：已完成
    }
}
