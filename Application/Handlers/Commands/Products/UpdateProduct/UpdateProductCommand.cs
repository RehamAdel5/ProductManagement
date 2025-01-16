using MediatR;

namespace Application.Handlers.Commands.Products.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public required string TitleAr { get; set; }
        public required string TitleEn { get; set; }
        public string? Image { get; set; }
        public Guid CategoryId { get; set; }
    }
}
