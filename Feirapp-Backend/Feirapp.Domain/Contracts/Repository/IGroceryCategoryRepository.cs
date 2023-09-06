using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryCategoryRepository
{
    Task<List<GroceryCategory>> GetAllGroceryCategories();
    Task<GroceryCategory> GetGroceryCategoryById(string id);
    Task<GroceryCategory> CreateGroceryCategory(GroceryCategory groceryCategory);
    Task UpdateGroceryCategory(GroceryCategory groceryCategory);
    Task DeleteGroceryCategory(string id);
}