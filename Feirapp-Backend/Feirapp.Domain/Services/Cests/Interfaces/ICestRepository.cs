using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Cests.Interfaces;

public interface ICestRepository
{
    Task InsertListOfCodesAsync(List<string?> cestCodes, CancellationToken ct);
    Task<Cest> AddIfNotExistsAsync(Func<Cest, bool> func, Cest cest, CancellationToken ct);
}