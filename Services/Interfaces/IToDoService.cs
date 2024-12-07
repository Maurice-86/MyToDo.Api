using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Services.Interfaces
{
    public interface IToDoService : IBaseService<TodoDto>
    {
        // 可以添加 ToDo 特有的方法
        Task<ApiResponse> GetAllByStatus(int status);
    }
}
