﻿using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemoController : ControllerBase
    {
        private readonly IMemoService service;

        public MemoController(IMemoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) =>
            await service.GetByIdAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll() =>
            await service.GetAllAsync();

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto model) =>
            await service.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MemoDto model) =>
            await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await service.DeleteAsync(id);
    }
}
