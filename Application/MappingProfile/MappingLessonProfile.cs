using Application.DtoModels.Lessons.Lessons;
using Application.DtoModels.Lessons.Phrasees;
using Application.DtoModels.Lessons.Quiz;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingLessonProfile : Profile
    {
        public MappingLessonProfile()
        {
            CreateMap<Lesson, LessonDto>()
                .ForMember(dest => dest.Words, opt => opt.MapFrom(src => src.Words))
                .ForMember(dest => dest.Phrases, opt => opt.MapFrom(src => src.Phrases))
                .ForMember(dest => dest.Quizzes, opt => opt.MapFrom(src => src.Quizzes));

            CreateMap<Lesson, LessonDetailDto>()
                .IncludeBase<Lesson, LessonDto>();

            CreateMap<LessonWord, LessonWordDto>()
                .ForMember(dest => dest.IsAdditional, opt => opt.MapFrom(src => src.Type == "additional"));

            CreateMap<LessonPhrase, LessonPhraseDto>();

            CreateMap<Quiz, QuizDto>()
                .ForMember(dest => dest.QuizType, opt => opt.MapFrom(src => src.Type.ToUpper()))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<QuizQuestion, QuizQuestionDto>()
                .ForMember(dest => dest.HasMedia,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ImageUrl) ||
                                              !string.IsNullOrEmpty(src.AudioUrl)))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<QuizAnswer, QuizAnswerDto>();

            CreateMap<CreateLessonDto, Lesson>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateLessonDto, Lesson>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}