
using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Extensions;
using MyToDo.Api.Service;

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
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IToDoService, ToDoService>();
            builder.Services.AddTransient<IMemoService, MemoService>();

            // ���� AutoMapper
            var autoMapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProFile());
            });
            builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

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
