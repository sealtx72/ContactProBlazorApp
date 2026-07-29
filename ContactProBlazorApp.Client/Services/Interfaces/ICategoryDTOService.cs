using ContactProBlazorApp.Client.Models;

namespace ContactProBlazorApp.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        //Create
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO, string userId);
    }
}
