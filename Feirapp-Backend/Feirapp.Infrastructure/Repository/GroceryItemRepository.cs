using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Extensions;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository : BaseRepository<GroceryItem>, IGroceryItemRepository, IDisposable
{
    private readonly BaseContext _context;

    public GroceryItemRepository(BaseContext context) : base(context)
    {
        var options = new DbContextOptions<BaseContext>();
        _context = context ?? new BaseContext(options);
    }

    public async Task<List<GroceryItem>> GetRandomGroceryItems(int quantity, CancellationToken ct)
    {
        var result = await _context.GroceryItems
            .OrderBy(x => Guid.NewGuid())
            .Take(quantity)
            .AsNoTracking()
            .ToListAsync(ct);
        return result;
    }

    public async Task<GroceryItem?> GetByBarcodeAndStoreIdAsync(string itemBarcode, long itemStoreId, CancellationToken ct)
    {
        return await _context.GroceryItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Barcode == itemBarcode && x.StoreId == itemStoreId, ct).ConfigureAwait(false);
    }

    public async Task InsertPriceLogAsync(PriceLog priceLog, CancellationToken ct)
    {
        await _context.PriceLogs.AddIfNotExistsAsync(priceLog,
            p => p.GroceryItemId == priceLog.Id && p.LogDate.Date == priceLog.LogDate.Date,
            ct).ConfigureAwait(false);
    }

    public async Task<PriceLog?> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct)
    {
        return await _context.PriceLogs
            .AsNoTracking()
            .OrderByDescending(x => x.LogDate)
            .FirstOrDefaultAsync(x => x.GroceryItemId == groceryItemId, ct);
    }

    public void Dispose()
    {
    }
}