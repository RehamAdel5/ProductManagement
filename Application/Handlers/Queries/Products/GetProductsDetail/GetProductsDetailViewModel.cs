using Domain.Entities;

namespace Application.Handlers.Queries.Products.GetProductsDetail
{
    public class GetProductsDetailViewModel
    {
        public Guid Id { get; set; }
        public required string TitleAr { get; set; }
        public required string TitleEn { get; set; }
        public string? Image { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
