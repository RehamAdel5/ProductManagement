using Domain.Abstractions;
using Domain.Entities;
using External.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> ListCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        { 
            return await _context.Categories.ToListAsync();
        }
        public async Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(item => item.CategoryId == categoryId)
                .ToListAsync();
        }
        public async Task<Category?> GetCategoryAsync(Guid id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == id);
        }
        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
             var category = await GetCategoryAsync(id);
            if (category is null) return false;

            _context.Categories.Remove(category);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            category.Id = Guid.NewGuid();
            var createdEntity = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return createdEntity.Entity;
        }


        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var result = _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
