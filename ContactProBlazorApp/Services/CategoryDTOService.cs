using ContactProBlazorApp.Services.Interfaces;
using ContactProBlazorApp.Client.Services.Interfaces;
using ContactProBlazorApp.Client.Models;
using ContactProBlazorApp.Models;

namespace ContactProBlazorApp.Services
{
    public class CategoryDTOService(ICategoryRepository repository) : ICategoryDTOService
    {
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            Category newCategory = new()
            {
                AppUserId = userId,
                Name = category.Name
            };

            newCategory = await repository.CreateCategoryAsync(newCategory);

            return newCategory.ToDTO();
        }
    }
}
