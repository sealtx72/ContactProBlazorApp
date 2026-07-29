using ContactProBlazorApp.Data;
using ContactProBlazorApp.Models;
using ContactProBlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ContactProBlazorApp.Services
{
    public class CategoryRepository(IDbContextFactory<ApplicationDbContext> ContextFactory) : ICategoryRepository
    {
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            using ApplicationDbContext context = ContextFactory.CreateDbContext();
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }
    }
}
