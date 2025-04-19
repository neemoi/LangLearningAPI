using Application.DtoModels.Pronunciation;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingPronunciationMappingProfile : Profile
    {
        public MappingPronunciationMappingProfile()
        {
            CreateMap<PronunciationCategory, PronunciationCategoryDto>();
            CreateMap<CreateCategoryDto, PronunciationCategory>();
            CreateMap<UpdateCategoryDto, PronunciationCategory>();

            CreateMap<WordItem, WordItemDto>();
            CreateMap<CreateWordItemDto, WordItem>();
            CreateMap<UpdateWordItemDto, WordItem>();
        }
    }
}
