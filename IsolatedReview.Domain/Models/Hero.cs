namespace LightPeak.Domain.Models
{
    public class Hero
    {
        public Guid HeroId { get; set; }
        public string Name { get; set; }

        public Hero(string name)
        {
            HeroId = Guid.NewGuid();
            Name = name;
        }
    }
}