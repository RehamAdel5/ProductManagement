namespace Domain.Entities
{
    public class Product: Entity
    {
        public required string TitleAr { get; set; }
        public required string TitleEn { get; set; }
        public string? Image { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
