using Feirapp.Domain.Models;

namespace Feirapp.Domain.Contracts.Service
{
    public interface IPriceLogService
    {
        Task<PriceLogModel> GetByIdAsync(string id);

        Task<List<PriceLogModel>> GetAllAsync();

        Task<PriceLogModel> InsertAsync(PriceLogModel priceLogModel);

        Task UpdateAsync(PriceLogModel priceLogModel);

        Task DeleteAsync(string id);
    }
}