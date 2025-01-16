using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Queries.Products.GetProductsList
{
    public class GetProductsListQuery : IRequest<List<GetProductsListViewModel>>
    {
        public Guid? CategoryId { get; set; }
        public GetProductsListQuery(Guid? categoryId) 
        { CategoryId = categoryId; }
    }
}
