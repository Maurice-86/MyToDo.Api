﻿using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services.Implementations;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("Api/[controller]/[action]")]
    public class MemoController : ControllerBase
    {
        private readonly MemoService service;

        public MemoController(MemoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int uid, int id) =>
            await service.GetByIdAsync(uid, id);

        [HttpGet]
        public async Task<ApiResponse> GetAll(int uid) =>
            await service.GetAllAsync(uid);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto model) =>
            await service.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] MemoDto model) =>
            await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int uid, int id) =>
            await service.DeleteAsync(uid, id);
    }
}
