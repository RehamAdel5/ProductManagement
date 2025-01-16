using AutoMapper;
using Domain.Abstractions;
using MediatR;

namespace Application.Handlers.Queries.Products.GetProductsList
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, List<GetProductsListViewModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<List<GetProductsListViewModel>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = request.CategoryId.HasValue ? await _productRepository.GetProductsByCategoryId(request.CategoryId.Value) : await _productRepository.ListProducts();
            return _mapper.Map<List<GetProductsListViewModel>>(products);
        }
    }
}
