using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using LinqKit;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services.Implementations
{
    public class ToDoService : BaseService<TodoDto, ToDo>, IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> GetAllByQueryParameter(QueryParameter queryParameter)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();

                // 使用 LINQ 根据查询参数构建筛选条件
                var predicate = PredicateBuilder.New<ToDo>(x => true);

                if (!string.IsNullOrEmpty(queryParameter.Keyword))
                    predicate = predicate.And(x => x.Title.Contains(queryParameter.Keyword));

                if (queryParameter.Status.HasValue)
                    predicate = predicate.And(x => x.Status.Equals(queryParameter.Status.Value));

                var entities = await repository.GetAllAsync(predicate: predicate);
                var dtos = mapper.Map<IEnumerable<TodoDto>>(entities);
                return new ApiResponse<IEnumerable<TodoDto>>("获取成功", dtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse($"获取失败{ex.Message}");
            }
        }
    }
}
