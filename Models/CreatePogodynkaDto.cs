using System.ComponentModel.DataAnnotations;

namespace PogodynkaAPI.Models
{
    public class CreatePogodynkaDto
    {
        public int? TempC { get; set; }
        public int? TempF { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
        public byte[]? ImageData { get; set; }
    }
}
