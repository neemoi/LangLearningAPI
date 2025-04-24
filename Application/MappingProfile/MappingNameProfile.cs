using Application.DtoModels.Name.EnglishName;
using Application.DtoModels.Name.FemaleName;
using Application.DtoModels.Name.MaleName;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingNameProfile : Profile
    {
        public MappingNameProfile()
        {
            CreateMap<EnglishName, EnglishNameDto>().ReverseMap();
            CreateMap<CreateEnglishNameDto, EnglishName>();
            CreateMap<UpdateEnglishNameDto, EnglishName>();

            CreateMap<MaleName, MaleNameDto>()
                .ForMember(dest => dest.EnglishName, opt => opt.MapFrom(src => src.EnglishName));
            CreateMap<CreateMaleNameDto, MaleName>();
            CreateMap<UpdateMaleNameDto, MaleName>();

            CreateMap<FemaleName, FemaleNameDto>().ReverseMap();
            CreateMap<CreateFemaleNameDto, FemaleName>();
            CreateMap<UpdateFemaleNameDto, FemaleName>();

            CreateMap<EnglishName, EnglishNameDto>();
            CreateMap<FemaleName, FemaleNameDto>();

            CreateMap<FemaleName, FemaleNameDto>()
                          .ForMember(dest => dest.EnglishName, opt => opt.MapFrom(src => src.EnglishName));
            CreateMap<CreateFemaleNameDto, FemaleName>();
            CreateMap<UpdateFemaleNameDto, FemaleName>();
        }
    }
}