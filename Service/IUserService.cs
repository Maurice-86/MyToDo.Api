using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public interface IUserService
    {
        Task<ApiResponse> LoginAsync(string username, string password);

        Task<ApiResponse> RegisterAsync(UserDto user);
    }
}
