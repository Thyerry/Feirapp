using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Models;

namespace Feirapp.Domain.Services;

public class GroceryCategoryService : IGroceryCategoryService
{
    private readonly IGroceryCategoryRepository _repository;

    public GroceryCategoryService(IGroceryCategoryRepository repository)
    {
        _repository = repository;
    }


    public Task<GroceryCategoryModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GroceryCategoryModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<GroceryCategoryModel> InsertAsync(GroceryCategoryModel groceryCategory, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(GroceryCategoryModel groceryCategory, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}