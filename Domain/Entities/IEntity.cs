namespace MyToDo.Api.Domain.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
        int Uid { get; set; }
        long CreateTime { get; set; }
        long UpdateTime { get; set; }
    }
}
