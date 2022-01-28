using System.ComponentModel.DataAnnotations;

namespace PogodynkaAPI.Models
{
    public class PogodynkaDto
    {
        public int Id { get; set; }
        public int TempC { get; set; }
        public int TempF { get; set; }
        public DateTime DateTime { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
        public byte[]? ImageData { get; set; }
    }
}
