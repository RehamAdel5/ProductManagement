using MediatR;

namespace Application.Handlers.Queries.Products.GetProductsDetail
{
    public class GetProductsDetailQuery : IRequest<GetProductsDetailViewModel>
    {
        public Guid Id { get; set; }
        public GetProductsDetailQuery(Guid id) { Id = id; }

    }
}
