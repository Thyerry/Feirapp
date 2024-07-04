using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Feirapp.Domain.Services.BaseRepository;

/// <summary>
/// Interface for a generic repository providing basic CRUD operations and more.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IBaseRepository<T>
{
    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the transaction.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    
    /// <summary>
    /// Inserts a new entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted entity.</returns>
    Task<T> InsertAsync(T entity, CancellationToken ct);
    
    /// <summary>
    /// Inserts a list of new entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task InsertListAsync(List<T> entities, CancellationToken ct);
    
    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(long id, CancellationToken ct);
    
    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(T entity, CancellationToken ct);
    
    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of entities.</returns>
    Task<List<T>> GetAllAsync(CancellationToken ct);
    
    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, null otherwise.</returns>
    Task<T?> GetByIdAsync(long id, CancellationToken ct);

    /// <summary>
    /// Runs the predicate and see if the entity exists,
    /// if it does, it returns the entity,
    /// if it doesn't, it adds the entity and returns it.
    /// </summary>
    /// <param name="predicate">Query to find the item on database</param>
    /// <param name="entity">Entity to insert</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Entity found or inserted</returns>
    Task<T> AddIfNotExistsAsync(Func<T, bool> predicate, T entity, CancellationToken ct = default);

    /// <summary>
    /// Retrieves entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>A list of entities that match the specified predicate.</returns>
    List<T> GetByQuery(Func<T, bool> predicate, CancellationToken ct);
}