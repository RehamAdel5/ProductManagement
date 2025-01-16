using MediatR;

namespace Application.Handlers.Commands.Products.DeleteProduct
{
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
