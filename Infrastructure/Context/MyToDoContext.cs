using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Domain.Entities;

namespace MyToDo.Api.Infrastructure.Context
{
    public class MyToDoContext : DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Memo> Memos { get; set; }

    }
}
