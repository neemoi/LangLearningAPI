using Application.DtoModels.Auth;
using Application.DtoModels.Auth.Response;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingAuthProfile : Profile
    {
        public MappingAuthProfile()
        {
            CreateMap<AuthRegisterDto, Users>();
            CreateMap<Users, AuthRegisterResponseDto>();
            CreateMap<Users, AuthLoginResponseDto>();
            CreateMap<Users, UserStatusResponseDto>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
             .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => src.LockoutEnd != null && src.LockoutEnd > DateTimeOffset.UtcNow));
        }
    }
}