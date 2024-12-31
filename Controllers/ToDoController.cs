using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Services.Implementations;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("Api/[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService service;

        public ToDoController(ToDoService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ApiResponse> Get(int uid, int id) =>
            await service.GetByIdAsync(uid, id);

        [HttpGet]
        public async Task<ApiResponse> GetAll(int uid) =>
            await service.GetAllAsync(uid);

        [HttpGet]
        public async Task<ApiResponse> GetAllByQueryParameter(int uid, [FromQuery] QueryParameter queryParameter) =>
            await service.GetAllByQueryParameter(uid, queryParameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] TodoDto model) =>
            await service.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] TodoDto model) =>
            await service.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int uid, int id) =>
            await service.DeleteAsync(uid, id);
    }
}
