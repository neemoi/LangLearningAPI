using Application.DtoModels.Lessons.Words;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingLessonWordProfile : Profile
    {
        public MappingLessonWordProfile()
        {
            CreateMap<LessonWord, LessonWordDto>();
            CreateMap<CreateLessonWordDto, LessonWord>();
            CreateMap<UpdateLessonWordDto, LessonWord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
