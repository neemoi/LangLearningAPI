using Application.DtoModels.Nouns;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfile
{
    public class MappingNounsProfile : Profile
    {
        public MappingNounsProfile()
        {
            CreateMap<AlphabetLetter, AlphabetLetterDto>().ReverseMap();
            CreateMap<NounWord, NounWordDto>().ReverseMap();
            CreateMap<CreateAlphabetLetterDto, AlphabetLetter>();
            CreateMap<UpdateAlphabetLetterDto, AlphabetLetter>();
            CreateMap<NounWord, NounWordDto>().ReverseMap();
            CreateMap<NounWordDto, NounWord>();
        }
    }
}
