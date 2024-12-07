using Arch.EntityFrameworkCore.UnitOfWork;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Infrastructure.Context;

namespace MyToDo.Api.Infrastructure.Repository
{
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyToDoContext dbContext) : base(dbContext)
        {

        }
    }
}
