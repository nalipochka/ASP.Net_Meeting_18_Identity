using ASP.Net_Meeting_18_Identity.Data;
using ASP.Net_Meeting_18_Identity.Models.DTOs.UserDTOs;
using AutoMapper;

namespace ASP.Net_Meeting_18_Identity.AutoMapperProfiles
{
    public class UserProfiles :Profile
    {
        public UserProfiles() 
        {
            CreateMap<User, UserDTO>().ForMember(dest=> dest.Login, opt=>opt.MapFrom(src=>src.UserName)).ReverseMap();
            CreateMap<User, EditUserDTO>().ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<User, DeleteUserDTO>().ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<User, CreateUserDTO>().ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<User, ChangePasswordDTO>().ReverseMap();
        }
    }
}
