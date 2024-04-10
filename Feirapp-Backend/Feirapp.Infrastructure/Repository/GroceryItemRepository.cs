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

    public async Task<List<GroceryItem>> InsertBatchAsync(List<GroceryItem> groceryItems, CancellationToken ct)
    {
        // await _context.Database.BeginTransactionAsync(ct);
        var store = groceryItems.Select(x => x.Store).FirstOrDefault();

        var ncms = groceryItems
            .Where(g => g.NcmCode != null)
            .Select(x => x.NcmCode)
            .Distinct()
            .Except(_context.Ncms.Select(s => s.Code));

        var cests = groceryItems
            .Where(g => g.CestCode != null)
            .Select(g => g.CestCode)
            .Distinct()
            .Except(_context.Cests.Select(s => s.Code));

        if (store != null)
            await _context.Stores.AddAsync(store, ct);

        if (ncms.Any())
            await _context.Ncms.AddRangeAsync(ncms.Select(s => new Ncm { Code = s }), ct);

        if (cests.Any())
            await _context.Cests.AddRangeAsync(cests.Select(s => new Cest { Code = s }), ct);

        foreach (var groceryItem in groceryItems)
        {
            var dbItem = await _context.GroceryItems
                .FirstOrDefaultAsync(x => x.Barcode == groceryItem.Barcode && x.StoreId == groceryItem.StoreId, ct);

            if (dbItem == null)
            {
                await _context.GroceryItems.AddAsync(groceryItem, ct);
                await _context.PriceLogs.AddAsync(new PriceLog
                {
                    GroceryItemId = groceryItem.Id,
                    Price = groceryItem.Price,  
                    LogDate = groceryItem.PurchaseDate,
                }, ct);
            }
            
            if (groceryItem.Price != dbItem?.Price && dbItem != null)
            {
                dbItem.Price = groceryItem.Price;
                dbItem.LastUpdate = DateTime.Now;
                _context.GroceryItems.Update(dbItem);
                var priceLog = new PriceLog
                {
                    GroceryItemId = dbItem.Id,
                    Price = dbItem.Price,
                    LogDate = dbItem.PurchaseDate,
                };
                await _context.PriceLogs.AddIfNotExistsAsync(priceLog, 
                    p => p.GroceryItemId == priceLog.GroceryItemId 
                         && p.LogDate.Date.Equals(priceLog.LogDate.Date)
                    , ct);
            }
        }

        // await _context.Database.CommitTransactionAsync(ct);
        await _context.SaveChangesAsync(ct);
        return groceryItems;
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