using System.Diagnostics;
using Feirapp.DocumentModels;
using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Dtos;
using Feirapp.Domain.Mappers;
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

    public async Task<List<GroceryCategoryDto>> SearchAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default)
    {
        return (await _repository.SearchAsync(groceryCategory.ToModel(), cancellationToken)).ToDtoList();
    }

    public async Task<List<GroceryCategoryDto>> InsertBatch(List<GroceryCategoryDto> groceryCategoryDtos, CancellationToken cancellationToken = default)
    {
        var validator = new InsertGroceryCategoryValidator();
        foreach (var category in groceryCategoryDtos)
        {
            await validator.ValidateAndThrowAsync(category, cancellationToken);
        }

        var result = await _repository.InsertBatchAsync(groceryCategoryDtos.ToModelList(), cancellationToken);
        return result.ToDtoList();
    }
}