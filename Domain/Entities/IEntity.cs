namespace MyToDo.Api.Domain.Entities
{
    public interface IEntity
    {
        public int Id { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
    }
}
