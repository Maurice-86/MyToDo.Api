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
            CreateMap<Memo, MemoDto>().ReverseMap();
        }
    }
}
