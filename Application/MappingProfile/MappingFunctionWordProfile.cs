using Application.DtoModels.Functions;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingFunctionWordProfile : Profile
    {
        public MappingFunctionWordProfile()
        {
            CreateMap<FunctionWord, FunctionWordDto>();
            CreateMap<CreateFunctionWordDto, FunctionWord>();
            CreateMap<FunctionWordUpdateDto, FunctionWord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PartOfSpeech, PartOfSpeechDto>();
            CreateMap<CreatePartOfSpeechDto, PartOfSpeech>();
            CreateMap<FunctionWordUpdateDto, PartOfSpeech>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
