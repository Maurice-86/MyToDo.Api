using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services.Interfaces
{
    public interface IMemoService : IBaseService<MemoDto>
    {
        // 可以添加 Memo 特有的方法
    }
}
