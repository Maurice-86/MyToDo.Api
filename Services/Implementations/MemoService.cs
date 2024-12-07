using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;
using MyToDo.Shared.Parameters;

namespace MyToDo.Api.Services.Implementations
{
    public class MemoService : BaseService<MemoDto, Memo>, IMemoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public override async Task<ApiResponse> AddAsync(MemoDto model)
        {
            model.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            return await base.AddAsync(model);
        }

        public override async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            model.UpdateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            return await base.UpdateAsync(model);
        }
    }
}
