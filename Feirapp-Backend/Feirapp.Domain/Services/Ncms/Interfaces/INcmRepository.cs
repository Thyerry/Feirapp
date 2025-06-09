using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Ncms.Interfaces;

public interface INcmRepository
{
    Task<Ncm?> GetByCodeAsync(string code, CancellationToken ct);
    Task InsertListOfCodesAsync(List<string> ncmCodes, CancellationToken ct);
    Task<List<Ncm>> GetByQuery(Func<Ncm, bool> func, CancellationToken ct);
    Task UpdateAsync(Ncm ncmUpdate, CancellationToken ct);
}