using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Domain.Entities;

namespace MyToDo.Api.Infrastructure.Context
{
    public class MyToDoContext : DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options)
        {

        }

        /// <summary>
        /// 重写SaveChangesAsync方法，在保存到数据库前自动处理实体的时间戳
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IEntity);

            var now = DateTimeOffset.Now.ToUnixTimeSeconds();

            foreach (var entry in entries)
            {
                var entity = (IEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreateTime = now;
                        break;
                    case EntityState.Modified:
                        entity.UpdateTime = now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<ToDo>? ToDos { get; set; }
        public DbSet<Memo>? Memos { get; set; }

    }
}
