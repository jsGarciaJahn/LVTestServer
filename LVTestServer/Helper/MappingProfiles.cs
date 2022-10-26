using AutoMapper;
using LVTestServer.Dto;
using LVTestServer.Models;

namespace LVTestServer.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
