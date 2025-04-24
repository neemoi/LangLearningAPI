namespace Application.DtoModels.Name.MaleName
{
    public class UpdateMaleNameDto
    {
        public int Id { get; set; }

        public int EnglishNameId { get; set; }

        public string Name { get; set; } = null!;
    }
}
