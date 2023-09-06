using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IGroceryCategoryRepository
{
    Task<List<GroceryCategory>> GetAllGroceryCategories();
    Task<GroceryCategory> GetGroceryCategoryById(int id);
}
