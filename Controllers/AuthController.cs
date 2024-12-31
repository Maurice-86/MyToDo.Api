using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("Api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto user) =>
            await service.LoginAsync(user.Username, user.Password);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto user) =>
            await service.RegisterAsync(user);

        [HttpPost]
        public async Task<ApiResponse> RefreshToken([FromBody] string refreshToken) =>
            await service.RefreshTokenAsync(refreshToken);
    }
}
