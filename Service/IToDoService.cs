using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    public interface IToDoService
    {
        Task<ApiResponse> GetAsync(int id);

        Task<ApiResponse> GetAllAsync(QueryParameter param);

        Task<ApiResponse> AddAsync(ToDoDto toDo);

        Task<ApiResponse> UpdateAsync(int id, ToDoDto toDo);

        Task<ApiResponse> DeleteAsync(int id);
    }
}
