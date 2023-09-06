using Feirapp.Entities;

namespace Feirapp.Domain.Contracts.Repository;

public interface IPriceLogRepository
{
    Task<PriceLog> GetByIdAsync(string id);

    Task<List<PriceLog>> GetAllAsync();

    Task<PriceLog> InsertAsync(PriceLog priceLog);

    Task UpdateAsync(PriceLog priceLog);

    Task DeleteAsync(string id);
}