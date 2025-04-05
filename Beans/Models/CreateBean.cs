namespace Beans.Models
{
    public class CreateBean
    {
        public string _id { get; set; } = default!;
        public int Index { get; set; }
        public bool IsBOTD { get; set; }
        public decimal Cost { get; set; }
        public string? Image { get; set; }
        public string? Colour { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Country { get; set; }
    }
}
