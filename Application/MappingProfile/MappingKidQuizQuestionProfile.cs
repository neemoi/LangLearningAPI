using Application.DtoModels.KidQuiz;
using Application.DtoModels.KidQuiz.KidQuizQuestion;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingKidQuizQuestionProfile : Profile
    {
        public MappingKidQuizQuestionProfile()
        {
            CreateMap<CreateKidQuizQuestionDto, KidQuizQuestion>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.QuizTypeId, opt => opt.MapFrom(src => src.QuizTypeId))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.AudioUrl))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                .ForMember(dest => dest.WordCardId, opt => opt.MapFrom(src => src.WordCardId));  

            CreateMap<KidQuizQuestion, KidQuizQuestionDto>()
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.QuizTypeId, opt => opt.MapFrom(src => src.QuizTypeId))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
                .ForMember(dest => dest.AudioUrl, opt => opt.MapFrom(src => src.AudioUrl))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer));

            CreateMap<UpdateKidQuizQuestionDto, KidQuizQuestion>();
        }
    }
}
