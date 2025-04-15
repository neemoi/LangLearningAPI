using Application.DtoModels.Lessons.Progress;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingUserProgressProfile : Profile
    {
        public MappingUserProgressProfile()
        {

            CreateMap<UserProgress, UserProgressDto>().ReverseMap();

            CreateMap<UserProgressCreateUpdateDto, UserProgress>();
            CreateMap<UserWordProgressCreateDto, DtoModels.Lessons.Progress.UserWordProgress>();

            CreateMap<UserProgressCreateUpdateDto, UserProgress>();
            CreateMap<UserProgress, UserProgressResponseDto>()
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson.Title))
                .ForMember(dest => dest.QuizType, opt => opt.MapFrom(src => src.Quiz.Type));

            CreateMap<UserWordProgressCreateDto, DtoModels.Lessons.Progress.UserWordProgress>();
            CreateMap<DtoModels.Lessons.Progress.UserWordProgress, UserWordProgressResponseDto>();

            CreateMap<UserProgressCreateUpdateDto, UserProgress>();
            CreateMap<UserProgress, UserProgressCreateUpdateDto>();

            CreateMap<UserWordProgressCreateDto, Domain.Models.UserWordProgress>();
            CreateMap<Domain.Models.UserWordProgress, UserWordProgressResponseDto>()
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType.ToString()))
                .ForMember(dest => dest.WordText, opt => opt.MapFrom(src => src.Word.Name));
        }
    }
}