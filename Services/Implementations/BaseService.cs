using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Services.Implementations
{
    public class BaseService<T, TEntity> : IBaseService<T>
        where T : class
        where TEntity : class, IEntity
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;
        private readonly IRepository<TEntity> repository;

        public BaseService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
            this.repository = work.GetRepository<TEntity>();
        }

        public virtual async Task<ApiResponse> AddAsync(T model)
        {
            // virtual：表示可以被子类覆盖
            try
            {
                var entity = mapper.Map<TEntity>(model);
                await repository.InsertAsync(entity);
                if (await work.SaveChangesAsync() <= 0)
                    return new ApiResponse("添加失败");

                return new ApiResponse<T>("添加成功", mapper.Map<T>(entity));
            }
            catch (Exception ex)
            {
                return new ApiResponse($"添加失败：{ex.Message}");
            }
        }

        public virtual async Task<ApiResponse> DeleteAsync(int uid, int id)
        {
            try
            {
                var entity = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.Uid.Equals(uid) && x.Id.Equals(id));
                if (entity == null)
                    return new ApiResponse($"删除的数据不存在, 你输入的id={id}");

                repository.Delete(entity);
                if (await work.SaveChangesAsync() <= 0)
                    return new ApiResponse("删除失败");

                return new ApiResponse("删除成功", status: true);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"删除失败：{ex.Message}");
            }
        }

        public virtual async Task<ApiResponse> GetAllAsync(int uid)
        {
            try
            {
                var entities = await repository.GetAllAsync(predicate: 
                    x => x.Uid.Equals(uid));
                var models = mapper.Map<IEnumerable<T>>(entities);
                return new ApiResponse<IEnumerable<T>>("获取成功", models);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"获取失败：{ex.Message}");
            }
        }

        public virtual async Task<ApiResponse> GetByIdAsync(int uid, int id)
        {
            try
            {
                var entity = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.Uid.Equals(uid) && x.Id.Equals(id));
                if (entity == null)
                    return new ApiResponse("数据不存在");

                var model = mapper.Map<T>(entity);
                return new ApiResponse<T>("获取成功", model);
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

                // 检查是否存在
                if (await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(entity.Id)) == null)
                    return new ApiResponse("更新的数据不存在");

                repository.Update(entity);
                if (await work.SaveChangesAsync() <= 0)
                    return new ApiResponse("更新失败");

                return new ApiResponse<T>("更新成功", mapper.Map<T>(entity));
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
