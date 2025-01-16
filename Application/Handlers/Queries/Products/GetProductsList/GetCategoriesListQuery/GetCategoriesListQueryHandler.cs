using Domain.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Queries.Products.GetProductsList.GetCategoriesListQuery
{
    public class GetCategoriesListQueryHandler: IRequestHandler<GetCategoriesListQuery, List<Category>>
    {
        private readonly ICategoryRepository _categoryRepository; 
        public GetCategoriesListQueryHandler(ICategoryRepository categoryRepository) 
        { _categoryRepository = categoryRepository; 
        }
        public async Task<List<Category>> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.ListCategoriesAsync(); }
    }
}
