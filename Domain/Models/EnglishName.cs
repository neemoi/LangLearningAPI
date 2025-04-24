namespace Domain.Models
{
    public class EnglishName
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }
        
        public string? ImagePath { get; set; }

        public ICollection<FemaleName>? FemaleNames { get; set; }
        public ICollection<MaleName>? MaleNames { get; set; }
    }
}
