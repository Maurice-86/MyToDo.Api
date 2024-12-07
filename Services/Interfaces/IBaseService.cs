﻿using MyToDo.Shared.Models;

namespace MyToDo.Api.Services.Interfaces
{
    public interface IBaseService<T>
    {
        Task<ApiResponse> GetAllAsync();

        Task<ApiResponse> GetByIdAsync(int id);

        Task<ApiResponse> AddAsync(T model);

        Task<ApiResponse> UpdateAsync(T model);

        Task<ApiResponse> DeleteAsync(int id);
    }
}
