using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository(BaseContext context)
    : BaseRepository<GroceryItem>(context), IGroceryItemRepository, IDisposable
{
    private readonly BaseContext _context = context;

    public async Task<List<GroceryItem>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var result = await _context.GroceryItems
            .OrderBy(x => Guid.NewGuid())
            .Take(quantity)
            .Include(g => g.Store)
            .Include(g => g.PriceHistory)
            .ToListAsync(ct);
        return result;
    }

    public new async Task<List<GroceryItem>> GetAllAsync(CancellationToken ct)
    {
        var result = await _context.GroceryItems
            .Include(g => g.PriceHistory)
            .Include(g => g.Store)
            .ToListAsync(ct);
        
        return result;
    }
    
    public async Task<GroceryItem?> GetByBarcodeAndStoreIdAsync(string itemBarcode, long itemStoreId, CancellationToken ct)
    {
        return await _context.GroceryItems
            .FirstOrDefaultAsync(x => x.Barcode == itemBarcode && x.StoreId == itemStoreId, ct);
    }

    public async Task InsertPriceLogAsync(PriceLog priceLog, CancellationToken ct)
    {
        var result = await _context.PriceLogs
            .FirstOrDefaultAsync(x => x.GroceryItemId == priceLog.Id && x.LogDate.Date == priceLog.LogDate.Date, ct);
        if (result == null)
        {
            await _context.PriceLogs.AddAsync(priceLog, ct);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<PriceLog?> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct)
    {
        return await _context.PriceLogs
            .OrderByDescending(x => x.LogDate)
            .FirstOrDefaultAsync(x => x.GroceryItemId == groceryItemId, ct);
    }

    public async Task<List<GroceryItem>> GetByStoreAsync(long storeId, CancellationToken ct)
    {
        return await _context.GroceryItems
            .Where(x => x.StoreId == storeId)
            .Include(x => x.PriceHistory)
            .Include(x => x.Store)
            .ToListAsync(ct);
    }

    public void Dispose()
    {
    }
}