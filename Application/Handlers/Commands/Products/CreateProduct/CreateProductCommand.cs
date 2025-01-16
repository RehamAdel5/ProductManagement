using MediatR;

namespace Application.Handlers.Commands.Products.CreateProduct
{
    public class CreateProductCommand:IRequest<Guid>
    {
        public required string TitleAr { get; set; }
        public required string TitleEn { get; set; }
        public string? Image { get; set; }
        public Guid CategoryId { get; set; }
        
    }
}
