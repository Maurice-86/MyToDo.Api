using Arch.EntityFrameworkCore.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Common.Configurations;
using MyToDo.Api.Common.Extensions;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Infrastructure.Context;
using MyToDo.Api.Infrastructure.Repository;
using MyToDo.Api.Models.Validations;
using MyToDo.Api.Services.Implementations;
using MyToDo.Api.Services.Interfaces;

namespace MyToDo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 添加数据库上下文
            builder.Services.AddDbContext<MyToDoContext>(option =>
            {
                // appsettings.json
                var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
                option.UseSqlite(connectionString);
            }).AddUnitOfWork<MyToDoContext>()    // 添加工作单元
            .AddCustomRepository<User, UserRepository>()
            .AddCustomRepository<ToDo, ToDoRepository>()
            .AddCustomRepository<Memo, MemoRepository>();   // 添加仓储

            // 依赖注入
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoService, ToDoService>();
            builder.Services.AddScoped<IMemoService, MemoService>();

            // 配置 AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

            // 添加 FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<MemoValidator>();


            // 注册 JWT 配置
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));

            // 配置 JWT 服务
            builder.Services.AddJwtAuthentication(builder.Configuration);

            // 小写路由
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
