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
            .ToListAsync(ct);
        return result;
    }

    public new async Task<List<GroceryItem>> GetAllAsync(CancellationToken ct)
    {
        var result = await _context.GroceryItems
            .Join(_context.Stores,
                gi => gi.StoreId,
                s => s.Id,
                (groceryItem, store) => new 
                {
                    GroceryItem = groceryItem,
                    Store = store
                }).ToListAsync(ct);
        
        return result.Select(x => x.GroceryItem).ToList();
    }
    
    public async Task<GroceryItem?> GetByBarcodeAndStoreIdAsync(string itemBarcode, long itemStoreId, CancellationToken ct)
    {
        return await _context.GroceryItems
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
            .OrderByDescending(x => x.LogDate)
            .FirstOrDefaultAsync(x => x.GroceryItemId == groceryItemId, ct);
    }

    public async Task<List<GroceryItem>> GetByStoreIdAsync(long storeId, CancellationToken ct)
    {
        return await _context.GroceryItems
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);
    }

    public void Dispose()
    {
    }
}