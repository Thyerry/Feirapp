using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Models;
using Feirapp.Domain.Validators;
using FluentValidation;

namespace Feirapp.Domain.Services;

public class GroceryCategoryService : IGroceryCategoryService
{
    private readonly IGroceryCategoryRepository _repository;

    public GroceryCategoryService(IGroceryCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<GroceryCategoryModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(id, cancellationToken);
        return result.ToModel();
    }

    public async Task<List<GroceryCategoryModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(cancellationToken);
        return result.ToModelList();
    }

    public async Task<GroceryCategoryModel> InsertAsync(GroceryCategoryModel groceryCategory, CancellationToken cancellationToken = default)
    {
        var validator = new InsertGroceryCategoryValidator();
        await validator.ValidateAndThrowAsync(groceryCategory, cancellationToken);

        var result = await _repository.InsertAsync(groceryCategory.ToEntity(), cancellationToken);
        return result.ToModel();
    }

    public async Task UpdateAsync(GroceryCategoryModel groceryCategory, CancellationToken cancellationToken = default)
    {
        var validator = new UpdateGroceryCategoryValidator();
        await validator.ValidateAndThrowAsync(groceryCategory, cancellationToken);

        await _repository.UpdateAsync(groceryCategory.ToEntity(), cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }
}