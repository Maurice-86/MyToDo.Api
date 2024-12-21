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

            // ������ݿ�������
            builder.Services.AddDbContext<MyToDoContext>(option =>
            {
                // appsettings.json
                var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
                option.UseSqlite(connectionString);
            }).AddUnitOfWork<MyToDoContext>()    // ��ӹ�����Ԫ
            .AddCustomRepository<User, UserRepository>()
            .AddCustomRepository<ToDo, ToDoRepository>()
            .AddCustomRepository<Memo, MemoRepository>();   // ��Ӳִ�

            // ����ע��
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoService, ToDoService>();
            builder.Services.AddScoped<IMemoService, MemoService>();

            // ���� AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

            // ��� FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<MemoValidator>();


            // ע�� JWT ����
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));

            // ���� JWT ����
            builder.Services.AddJwtAuthentication(builder.Configuration);

            // Сд·��
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
