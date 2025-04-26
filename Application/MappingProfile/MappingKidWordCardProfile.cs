using Application.DtoModels.KidQuiz.KidWordCard;
using Application.DtoModels.KidQuiz;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingKidWordCardProfile : Profile
    {
        public MappingKidWordCardProfile()
        {
            CreateMap<CreateKidWordCardDto, KidWordCard>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.KidLessonId));

            CreateMap<UpdateKidWordCardDto, KidWordCard>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.KidLessonId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<KidWordCard, KidWordCardDto>()
                .ForMember(dest => dest.KidLessonId, opt => opt.MapFrom(src => src.LessonId));
        }
    }
}
