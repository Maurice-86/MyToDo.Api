using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Domain.Entities;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services.Implementations
{
    public class MemoService : BaseService<MemoDto, Memo>
    {
        public MemoService(IUnitOfWork work, IMapper mapper)
            : base(work, mapper) { }
    }
}
