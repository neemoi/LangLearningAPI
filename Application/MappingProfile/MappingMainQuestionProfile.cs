using Application.DtoModels.MainQuestions;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingMainQuestionProfile : Profile
    {
        public MappingMainQuestionProfile()
        {
            CreateMap<MainQuestion, MainQuestionDto>();
            CreateMap<CreateMainQuestionDto, MainQuestion>();
            CreateMap<UpdateMainQuestionDto, MainQuestion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<MainQuestionWord, MainQuestionWordDto>();
            CreateMap<CreateMainQuestionWordDto, MainQuestionWord>();
            CreateMap<UpdateMainQuestionWordDto, MainQuestionWord>() 
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}