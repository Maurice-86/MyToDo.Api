namespace MyToDo.Api.Context
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
    }
}
