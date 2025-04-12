using Application.DtoModels.Lessons.Lessons;
using Application.DtoModels.Lessons.Phrasees;
using Application.DtoModels.Lessons.Quiz;
using Application.DtoModels.Lessons.QuizQuestion;
using Application.DtoModels.Lessons.Words;
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

            CreateMap<CreateLessonDto, Lesson>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateLessonDto, Lesson>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<LessonWord, DtoModels.Lessons.Lessons.LessonWordDto>()
                .ForMember(dest => dest.IsAdditional, opt => opt.MapFrom(src => src.Type == "additional"));

            CreateMap<CreateLessonWordDto, LessonWord>();
            CreateMap<UpdateLessonWordDto, LessonWord>();

            CreateMap<LessonPhrase, LessonPhraseDto>();
            CreateMap<CreateLessonPhraseDto, LessonPhrase>();
            CreateMap<UpdateLessonPhraseDto, LessonPhrase>();

            CreateMap<Quiz, QuizDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToUpper()))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            CreateMap<CreateQuizDto, Quiz>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToUpper()));

            CreateMap<UpdateQuizDto, Quiz>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<QuizQuestion, QuizQuestionDto>()
                .ForMember(dest => dest.HasMedia,
                    opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ImageUrl) ||
                                              !string.IsNullOrEmpty(src.AudioUrl)))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<CreateQuizDto, QuizQuestion>();
            CreateMap<CreateQuizDto, QuizQuestion>();

            CreateMap<QuizAnswer, QuizAnswerDto>();
            CreateMap<CreateQuizDto, QuizAnswer>();
            CreateMap<CreateQuizDto, QuizAnswer>();


        }
    }
}