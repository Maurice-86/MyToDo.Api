using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Services.Implementations
{
    public class BaseService<T, TEntity> : IBaseService<T>
        where TEntity : class, IEntity
        where T : class
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public BaseService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public virtual async Task<ApiResponse> AddAsync(T model)
        {
            // virtual：表示可以被子类覆盖
            try
            {
                var entity = mapper.Map<TEntity>(model);
                var repository = work.GetRepository<TEntity>();
                await repository.InsertAsync(entity);
                if (await work.SaveChangesAsync() > 0)
                {
                    var dto = mapper.Map<T>(entity);
                    return new ApiResponse("添加成功", dto);
                }
                return new ApiResponse("添加失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public virtual async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<TEntity>();
                var entity = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.Id.Equals(id));
                if (entity == null)
                    return new ApiResponse("删除的数据不存在");

                repository.Delete(entity);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse("删除成功", status: true);

                return new ApiResponse("删除失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public virtual async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var repository = work.GetRepository<TEntity>();
                var entities = await repository.GetAllAsync();
                var dtos = mapper.Map<IEnumerable<T>>(entities);
                return new ApiResponse(dtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public virtual async Task<ApiResponse> GetByIdAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<TEntity>();
                var entity = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.Id.Equals(id));
                if (entity == null)
                    return new ApiResponse("数据不存在");

                var dto = mapper.Map<T>(entity);
                return new ApiResponse(dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public virtual async Task<ApiResponse> UpdateAsync(T model)
        {
            try
            {
                var entity = mapper.Map<TEntity>(model);
                var repository = work.GetRepository<TEntity>();

                // 检查是否存在
                if (await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(entity.Id)) == null)
                    return new ApiResponse("更新的数据不存在");

                repository.Update(entity);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse("更新成功", status: true);

                return new ApiResponse("更新失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
