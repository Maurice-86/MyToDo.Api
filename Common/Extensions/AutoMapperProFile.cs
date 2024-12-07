using AutoMapper;
using MyToDo.Api.Domain.Entities;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Common.Extensions
{
    public class AutoMapperProFile : Profile
    {
        public AutoMapperProFile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<ToDo, TodoDto>().ReverseMap();
            // DTO -> Entity (添加时使用)
            CreateMap<TodoDto, ToDo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())  // 忽略 Id
                .ForMember(dest => dest.CreateTime, opt =>
                    opt.MapFrom(src => DateTime.Now.Ticks))  // 设置创建时间
                .ForMember(dest => dest.UpdateTime, opt =>
                    opt.MapFrom(src => DateTime.Now.Ticks)); // 设置更新时间

            CreateMap<Memo, MemoDto>().ReverseMap();
        }
    }
}
