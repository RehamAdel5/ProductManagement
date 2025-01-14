using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IProductRepository
    {
        Task<Product> CreateProduct(Product product);
        Task<IEnumerable<Product>> GetProductsByCategoryId(Guid categoryId); 
        Task<bool> DeleteProduct(Guid id);
        Task<Product?> GetProduct(Guid id);
        Task<List<Product>> ListProducts();
        Task<Product> UpdateProduct(Product product);
    }
}
