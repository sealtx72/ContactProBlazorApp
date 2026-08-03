using ContactProBlazorApp.Client.Models;

namespace ContactProBlazorApp.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        //Create
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO, string userId);

        //Read
        Task<List<CategoryDTO>> GetCategoriesAsync(string userId);

        Task<CategoryDTO?> GetCategoryByIdAsync(int id, string userId);

        //Update
        Task UpdateCategoryAsync(CategoryDTO category, string userId);
    }
}
