using Application.DtoModels.KidQuiz.KidQuizAnswer;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingKidQuizAnswerProfile : Profile
    {
        public MappingKidQuizAnswerProfile()
        {
            CreateMap<KidQuizAnswer, KidQuizAnswerDto>().ReverseMap();

            CreateMap<CreateKidQuizAnswerDto, KidQuizAnswer>();

            CreateMap<UpdateKidQuizAnswerDto, KidQuizAnswer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}