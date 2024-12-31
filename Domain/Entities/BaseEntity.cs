namespace MyToDo.Api.Domain.Entities
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
    }
}
