using Domain.Abstractions;
using Domain.Entities;
using External.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> ListProducts()
        {
            return await _context.Products
                .AsNoTracking()
                .Include(product => product.Category)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(Guid categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetProduct(Guid id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == id);
        }
        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await GetProduct(id);
            if (product is null) return false;

            _context.Products.Remove(product);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            product.Id = Guid.NewGuid();
            var result = _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
