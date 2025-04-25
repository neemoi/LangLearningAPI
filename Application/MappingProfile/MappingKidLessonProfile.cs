using Application.DtoModels.KidQuiz.Lessons;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingKidLessonProfile : Profile
    {
        public MappingKidLessonProfile()
        {
            CreateMap<CreateKidLessonDto, KidLesson>()
                .ForMember(dest => dest.QuizQuestions, opt => opt.Ignore()) 
                .ForMember(dest => dest.WordCards, opt => opt.Ignore());

           
            CreateMap<UpdateKidLessonDto, KidLesson>()
                .ForMember(dest => dest.QuizQuestions, opt => opt.Ignore())
                .ForMember(dest => dest.WordCards, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); 

            CreateMap<KidLesson, KidLessonDto>()
                .ForMember(dest => dest.QuizQuestions, opt => opt.MapFrom(src => src.QuizQuestions))
                .ForMember(dest => dest.WordCards, opt => opt.MapFrom(src => src.WordCards));
        }
    }
}
