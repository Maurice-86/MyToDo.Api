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

        public async Task<ApiResponse> GetAllByStatus(int status)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var entities = await repository.GetAllAsync(predicate: x => x.Status.Equals(status));
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
