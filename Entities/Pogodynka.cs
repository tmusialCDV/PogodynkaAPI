namespace PogodynkaAPI.Entities
{
    public class Pogodynka
    {
        public int Id { get; set; }
        public int TempC { get; set; }
        public int TempF { get; set; }
        public DateTime DateTime { get; set; }
        public string? Description { get; set; }
        public byte[]? ImageData { get; set; }

        public int CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}
