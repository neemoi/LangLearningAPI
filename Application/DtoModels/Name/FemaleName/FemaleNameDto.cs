using Application.DtoModels.Name.EnglishName;

namespace Application.DtoModels.Name.FemaleName
{
    public class FemaleNameDto
    {
        public int Id { get; set; }
       
        public string? Name { get; set; }
        
        public int EnglishNameId { get; set; }
        
        public EnglishNameDto? EnglishName { get; set; }
    }
}
