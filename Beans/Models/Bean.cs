namespace Beans.Models
{
    public class Bean
    {
        public string _id { get; set; } = default!;
        public int Index { get; set; }
        public bool IsBOTD { get; set; }
        public string Cost { get; set; }
        public string? Image { get; set; }
        public string? Colour { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Country { get; set; }
    }
}
