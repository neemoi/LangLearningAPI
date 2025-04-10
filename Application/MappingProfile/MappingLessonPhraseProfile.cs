using Application.DtoModels.Lessons.Phrasees;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingLessonPhraseProfile : Profile
    {
        public MappingLessonPhraseProfile()
        {
            CreateMap<CreateLessonPhraseDto, LessonPhrase>()
                .ForMember(dest => dest.PhraseText, opt => opt.MapFrom(src => src.PhraseText))
                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId));

            CreateMap<UpdateLessonPhraseDto, LessonPhrase>()
                .ForMember(dest => dest.PhraseText, opt => opt.MapFrom(src => src.PhraseText))
                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

            CreateMap<LessonPhrase, LessonPhraseDto>()
                .ForMember(dest => dest.PhraseText, opt => opt.MapFrom(src => src.PhraseText))
                .ForMember(dest => dest.Translation, opt => opt.MapFrom(src => src.Translation))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId));
        }
    }
}
