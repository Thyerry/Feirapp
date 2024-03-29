﻿using Feirapp.Domain.Dtos;

namespace Feirapp.Domain.Contracts.Service;

public interface IGroceryCategoryService
{
    Task<GroceryCategoryDto> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<List<GroceryCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GroceryCategoryDto> InsertAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default);

    Task UpdateAsync(GroceryCategoryDto groceryCategoryDto, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    Task<List<GroceryCategoryDto>> SearchAsync(GroceryCategoryDto groceryCategory, CancellationToken cancellationToken = default);

    Task<List<GroceryCategoryDto>> InsertBatch(List<GroceryCategoryDto> groceryCategoryDtos, CancellationToken cancellationToken = default);
}