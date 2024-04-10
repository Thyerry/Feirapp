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

    public async Task<GroceryItem> GetByBarcodeAndStoreIdAsync(string itemBarcode, long itemStoreId, CancellationToken ct)
    {
        return await _context.GroceryItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Barcode == itemBarcode && x.StoreId == itemStoreId, ct).ConfigureAwait(false) ?? new GroceryItem();
    }

    public async Task InsertPriceLogAsync(PriceLog priceLog, CancellationToken ct)
    {
        await _context.PriceLogs.AddIfNotExistsAsync(priceLog,
            p => p.GroceryItemId == priceLog.Id && p.LogDate.Date == priceLog.LogDate.Date,
            ct).ConfigureAwait(false);
    }

    public async Task<PriceLog> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct)
    {
        return await _context.PriceLogs
            .AsNoTracking()
            .OrderByDescending(x => x.LogDate)
            .FirstOrDefaultAsync(x => x.GroceryItemId == groceryItemId, ct) ?? new PriceLog();
    }

    public new async Task<GroceryItem> InsertAsync(GroceryItem entity, CancellationToken ct)
    {
        await _context.Database.BeginTransactionAsync(ct);
        var store = entity.Store;

        if (entity.NcmCode != null && !_context.Ncms.Any(x => x.Code == entity.NcmCode))
            await _context.Ncms.AddAsync(new Ncm { Code = entity.NcmCode }, ct);

        if (entity.CestCode != null && !_context.Cests.Any(x => x.Code == entity.CestCode))
            await _context.Cests.AddAsync(new Cest { Code = entity.CestCode }, ct);

        if (store != null)
            await _context.Stores.AddAsync(store, ct);

        var dbItem = await _context.GroceryItems
            .FirstOrDefaultAsync(x => x.Barcode == entity.Barcode && x.StoreId == entity.StoreId, ct);

        if (dbItem == null)
        {
            entity = (await _context.GroceryItems.AddAsync(entity, ct)).Entity;
            await _context.PriceLogs.AddAsync(new PriceLog
            {
                GroceryItemId = entity.Id,
                Price = entity.Price,
                LogDate = entity.PurchaseDate,
            }, ct);
        }

        else if (entity.Price != dbItem.Price)
        {
            dbItem.Price = entity.Price;
            dbItem.LastUpdate = DateTime.Now;
            _context.GroceryItems.Update(dbItem);
            await _context.PriceLogs.AddAsync(new PriceLog
            {
                GroceryItemId = dbItem.Id,
                Price = dbItem.Price,
                LogDate = dbItem.PurchaseDate,
            }, ct);
        }

        await _context.SaveChangesAsync(ct);
        await _context.Database.CommitTransactionAsync(ct);
        return entity;
    }
    
    public void Dispose()
    {
    }
}