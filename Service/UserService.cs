﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> LoginAsync(string username, string password)
        {
            var model = await work.GetRepository<User>()
                .GetFirstOrDefaultAsync(predicate:
                x => x.UserName.Equals(username) &&
                x.Password.Equals(password));

            if (model == null)
                return new ApiResponse(-1, "登录失败，账号或密码错误！");
            return new ApiResponse(model);
        }

        public async Task<ApiResponse> RegisterAsync(UserDto user)
        {
            try
            {
                var model = mapper.Map<User>(user);
                var repository = work.GetRepository<User>();
                var dbModel = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.UserName.Equals(model.UserName) &&
                    x.Password.Equals(model.Password));

                if (dbModel != null)
                    return new ApiResponse(-1, "注册失败，账号已存在！");

                model.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                await repository.InsertAsync(model);
                if (work.SaveChanges() > 0)
                {
                    return new ApiResponse(model);
                }
                return new ApiResponse(-1, "注册失败，请重试！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, $"注册失败，{ex.Message}");
            }
        }
    }
}
