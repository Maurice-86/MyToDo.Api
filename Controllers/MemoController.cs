using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Controllers
{
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
            await service.GetAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameter param) =>
            await service.GetAllAsync(param);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MemoDto memo) =>
            await service.AddAsync(memo);

        [HttpPatch]
        public async Task<ApiResponse> Update(int id, [FromBody] MemoDto memo) =>
            await service.UpdateAsync(id, memo);

        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await service.DeleteAsync(id);
    }
}
