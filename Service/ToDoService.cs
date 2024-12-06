using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ToDoDto toDo)
        {
            try
            {
                var model = mapper.Map<ToDo>(toDo);
                var repository = work.GetRepository<ToDo>();

                model.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                await repository.InsertAsync(model);
                if (work.SaveChanges() > 0)
                {
                    return new ApiResponse(model);
                }
                return new ApiResponse(-1, "添加失败，请重试！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, $"添加失败，{ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var model = await work.GetRepository<ToDo>()
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                if (model == null)
                    return new ApiResponse();

                repository.Delete(model);

                if (work.SaveChanges() > 0)
                {
                    return new ApiResponse();
                }

                return new ApiResponse(-1, "删除失败，请重试！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, $"删除失败，{ex.Message}");
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter param)
        {
            var models = await work.GetRepository<ToDo>()
                .GetPagedListAsync(predicate:
                x => string.IsNullOrEmpty(param.Search) ? true : x.Title.Equals(param.Search),
                pageIndex: param.PageIndex,
                pageSize: param.PageSize);

            return new ApiResponse(models);
        }

        public async Task<ApiResponse> GetAsync(int id)
        {
            var model = await work.GetRepository<ToDo>()
                .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

            return new ApiResponse(model);
        }

        public async Task<ApiResponse> UpdateAsync(int id, ToDoDto toDo)
        {
            try
            {
                var model = mapper.Map<ToDo>(toDo);
                var repository = work.GetRepository<ToDo>();
                var dbModel = await work.GetRepository<ToDo>()
                    .GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));

                if (dbModel == null)
                    return new ApiResponse(-1, "更新失败，ToDo 不存在！");

                dbModel.Title = toDo.Title;
                dbModel.Content = toDo.Content;
                dbModel.Status = toDo.Status;
                dbModel.UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds();

                repository.Update(dbModel);

                if (work.SaveChanges() > 0)
                {
                    return new ApiResponse(dbModel);
                }

                return new ApiResponse(-1, "更新失败，请重试！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, $"更新失败，{ex.Message}");
            }
        }
    }
}
