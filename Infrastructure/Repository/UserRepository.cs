using Arch.EntityFrameworkCore.UnitOfWork;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Infrastructure.Context;

namespace MyToDo.Api.Infrastructure.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyToDoContext dbContext) : base(dbContext)
        {

        }
    }
}
