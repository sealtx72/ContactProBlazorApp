using ContactProBlazorApp.Models;

namespace ContactProBlazorApp.Services.Interfaces
{
    public interface ICategoryRepository
    {
        //Create
        Task<Category> CreateCategoryAsync(Category category);

        //Read
        Task<List<Category>> GetCategoriesAsync(string userId);

        Task<Category?> GetCategoryAsync(int id, string userId);

        //Update
        Task UpdateCategoryAsync(Category category, string userId);
    }
}
