using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand> 
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper) 
        { 
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        { 
            var product = _mapper.Map<Product>(request);
            await _productRepository.UpdateProduct(product);
            return Unit.Value;
        }
    }
}
