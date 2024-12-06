using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Login(string username, string password) =>
            await service.LoginAsync(username, password);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto user) =>
            await service.RegisterAsync(user);
    }
}
