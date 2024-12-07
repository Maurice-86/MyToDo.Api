using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse> LoginAsync(string username, string password);

        Task<ApiResponse> RegisterAsync(UserDto user);
    }
}
