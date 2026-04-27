using Authservice.Domain.Entites;
using AuthService.Application.DTOs;
using AutoMapper;
namespace AuthService.Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).ToArray()));

            CreateMap<SignUpDTO, User>();
        }
    }
}
