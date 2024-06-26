﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Feirapp.Domain.Services.BaseRepository;

public interface IBaseRepository<T>
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    Task<T> InsertAsync(T entity, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task<List<T>> GetAllAsync(CancellationToken ct);
    Task<T> GetByIdAsync(long id, CancellationToken ct);
    Task AddIfNotExistsAsync(T entity, Func<T, bool> predicate, CancellationToken ct = default);
    List<T> GetByQuery(Func<T, bool> predicate, CancellationToken ct);
}