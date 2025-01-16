using AutoMapper;
using Domain.Abstractions;
using MediatR;

namespace Application.Handlers.Queries.Products.GetProductsDetail
{
    public class GetProductDetailQueryHandler : IRequestHandler<GetProductsDetailQuery, GetProductsDetailViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductDetailQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<GetProductsDetailViewModel> Handle(GetProductsDetailQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProduct(request.Id);

            return _mapper.Map<GetProductsDetailViewModel>(product);
        }
    }
}
