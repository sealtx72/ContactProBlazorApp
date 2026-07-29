using ContactProBlazorApp.Models;

namespace ContactProBlazorApp.Services.Interfaces
{
    public interface ICategoryRepository
    {
        //Create
        Task<Category> CreateCategoryAsync(Category category);
    }
}
