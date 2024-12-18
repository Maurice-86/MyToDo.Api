﻿using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

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

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto user) =>
            await service.LoginAsync(user.UserName, user.Password);

        [HttpPost]
        public async Task<ApiResponse> Register([FromBody] UserDto user) =>
            await service.RegisterAsync(user);

        [HttpPost]
        public async Task<ApiResponse> RefreshToken([FromBody] string refreshToken) =>
            await service.RefreshTokenAsync(refreshToken);
    }
}
