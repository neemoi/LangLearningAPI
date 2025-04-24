namespace Domain.Models
{
    public class FemaleName
    {
        public int Id { get; set; }

        public int EnglishNameId { get; set; }

        public string Name { get; set; } = null!;

        public EnglishName? EnglishName { get; set; }
    }
}
