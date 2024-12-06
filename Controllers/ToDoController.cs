using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers
{
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
            await service.GetAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter param) =>
            await service.GetAllAsync(param);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] ToDoDto toDo) =>
            await service.AddAsync(toDo);

        [HttpPatch]
        public async Task<ApiResponse> Update(int id, [FromBody] ToDoDto toDo) =>
            await service.UpdateAsync(id, toDo);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await service.DeleteAsync(id);
    }
}
