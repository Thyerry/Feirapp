using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Ncms.Interfaces;

public interface INcmRepository : IBaseRepository<Ncm>
{
    Task<Ncm?> GetByCodeAsync(string code, CancellationToken ct);
    Task InsertListOfCodesAsync(List<string> entities, CancellationToken ct);
}