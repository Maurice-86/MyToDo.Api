using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;

namespace MyToDo.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService service;

        public ToDoController(IToDoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int id) =>
            await service.GetByIdAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll() =>
            await service.GetAllAsync();

        [HttpGet]
        public async Task<ApiResponse> GetAllByStatus(int status) =>
            await service.GetAllByStatus(status);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] TodoDto model) =>
            await service.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] TodoDto model) =>
            await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await service.DeleteAsync(id);
    }
}
