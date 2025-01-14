using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> CreateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<Category?> GetCategoryAsync(Guid id);
        Task<List<Category>> ListCategoriesAsync();
        Task<Category> UpdateCategoryAsync(Category category);
        Task<List<Product>?> GetProductsByCategoryIdAsync(Guid id);
    }
}
