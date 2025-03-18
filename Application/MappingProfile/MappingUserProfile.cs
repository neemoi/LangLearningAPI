using Application.DtoModels.AdminUsers;
using Application.DtoModels.User;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingUserProfile : Profile
    {
        public MappingUserProfile()
        {
            CreateMap<Users, UserByIdDto>();

            CreateMap<Users, UserDto>();

            CreateMap<Users, UserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<UpdateUserDto, Users>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl))
                .ForMember(dest => dest.BackgroundImageUrl, opt => opt.MapFrom(src => src.BackgroundImageUrl))
                .ForMember(dest => dest.RelationshipStatus, opt => opt.MapFrom(src => src.RelationshipStatus))
                .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests))
                .ForMember(dest => dest.FavoriteMovies, opt => opt.MapFrom(src => src.FavoriteMovies))
                .ForMember(dest => dest.FavoriteShows, opt => opt.MapFrom(src => src.FavoriteShows))
                .ForMember(dest => dest.FavoriteBooks, opt => opt.MapFrom(src => src.FavoriteBooks))
                .ForMember(dest => dest.FavoriteSports, opt => opt.MapFrom(src => src.FavoriteSports))
                .ForMember(dest => dest.FavoriteMusic, opt => opt.MapFrom(src => src.FavoriteMusic))
                .ForMember(dest => dest.PostalAddress, opt => opt.MapFrom(src => src.PostalAddress))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 
        }
    }
}