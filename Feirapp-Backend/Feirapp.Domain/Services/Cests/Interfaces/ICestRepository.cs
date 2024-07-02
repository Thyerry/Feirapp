using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Cests.Interfaces;

public interface ICestRepository : IBaseRepository<Cest>
{
    Task InsertListOfCodesAsync(List<string?> cestCodes, CancellationToken ct);
}