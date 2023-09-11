using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Mappers;
using Feirapp.Domain.Dtos;
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

    public async Task<GroceryCategoryDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(id, cancellationToken);
        return result.ToDto();
    }

    public async Task<List<GroceryCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(cancellationToken);
        return result.ToDtoList();
    }

    public async Task<GroceryCategoryDto> InsertAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default)
    {
        var validator = new InsertGroceryCategoryValidator();
        await validator.ValidateAndThrowAsync(groceryCategory, cancellationToken);

        var result = await _repository.InsertAsync(groceryCategory.ToModel(), cancellationToken);
        return result.ToDto();
    }

    public async Task UpdateAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default)
    {
        var validator = new UpdateGroceryCategoryValidator();
        await validator.ValidateAndThrowAsync(groceryCategory, cancellationToken);

        await _repository.UpdateAsync(groceryCategory.ToModel(), cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }
}