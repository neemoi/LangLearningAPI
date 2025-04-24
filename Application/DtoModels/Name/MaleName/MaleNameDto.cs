using Application.DtoModels.Name.EnglishName;

namespace Application.DtoModels.Name.MaleName
{
    public class MaleNameDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int EnglishNameId { get; set; }

        public EnglishNameDto? EnglishName { get; set; }
    }
}
