using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Service
{
    public interface IMemoService
    {
        Task<ApiResponse> GetAsync(int id);

        Task<ApiResponse> GetAllAsync(QueryParameter param);

        Task<ApiResponse> AddAsync(MemoDto memo);

        Task<ApiResponse> UpdateAsync(int id, MemoDto memo);

        Task<ApiResponse> DeleteAsync(int id);
    }
}
