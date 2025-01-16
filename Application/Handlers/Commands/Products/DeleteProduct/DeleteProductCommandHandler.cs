using Domain.Abstractions;
using MediatR;

namespace Application.Handlers.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{ 
    private readonly IProductRepository _productRepository;
    public DeleteProductCommandHandler(IProductRepository productRepository) 
    {
        _productRepository = productRepository;
    } 
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken) 
    {
        await _productRepository.DeleteProduct(request.Id); 
        return Unit.Value;
    }
}
}
