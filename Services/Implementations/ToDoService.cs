using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

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

        public override async Task<ApiResponse> AddAsync(TodoDto model)
        {
            model.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            return await base.AddAsync(model);
        }

        public override async Task<ApiResponse> UpdateAsync(TodoDto model)
        {
            model.UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            return await base.UpdateAsync(model);
        }

        public async Task<ApiResponse> GetAllByStatus(int status)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var entities = await repository.GetAllAsync(predicate: x => x.Status.Equals(status));
                var dtos = mapper.Map<IEnumerable<TodoDto>>(entities);
                return new ApiResponse(dtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        //public async Task<ApiResponse> GetAllAsync(QueryParameter param)
        //{
        //    var models = await work.GetRepository<ToDo>()
        //        .GetPagedListAsync(predicate:
        //        x => string.IsNullOrEmpty(param.Search) ? true : x.Title.Equals(param.Search),
        //        pageIndex: param.PageIndex,
        //        pageSize: param.PageSize);

        //    return new ApiResponse(models);
        //}
    }
}
