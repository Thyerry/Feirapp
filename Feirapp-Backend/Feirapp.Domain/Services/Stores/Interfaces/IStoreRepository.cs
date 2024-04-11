using Feirapp.Domain.Services.BaseRepository;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.Stores.Interfaces;

public interface IStoreRepository : IBaseRepository<Store>
{
    Task<Store?> GetByCnpjAsync(string? storeCnpj, CancellationToken ct);
}