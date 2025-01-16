using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;
namespace Application.Handlers.Commands.Products.CreateProduct
{
    public class AddProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    { 
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper) 
        { 
            _productRepository = productRepository; 
            _mapper = mapper;
        } 
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken) 
        { 
            var product = _mapper.Map<Product>(request); 
            await _productRepository.CreateProduct(product); return product.Id;
        } 
    }
}
