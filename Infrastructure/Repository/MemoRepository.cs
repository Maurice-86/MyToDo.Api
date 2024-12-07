using Arch.EntityFrameworkCore.UnitOfWork;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Infrastructure.Context;

namespace MyToDo.Api.Infrastructure.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyToDoContext dbContext) : base(dbContext)
        {

        }
    }
}
