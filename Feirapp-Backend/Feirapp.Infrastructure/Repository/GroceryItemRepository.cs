using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Entities.Dtos;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository(BaseContext context)
    : BaseRepository<GroceryItem>(context), IGroceryItemRepository, IDisposable
{
    private readonly BaseContext _context = context;

    public async Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsQuery queryParams,
        CancellationToken ct)
    {
        var query =
            from groceryItem in _context.GroceryItems
            join priceLog in _context.PriceLogs on groceryItem.Id equals priceLog.GroceryItemId
            join store in _context.Stores on priceLog.StoreId equals store.Id
            select new SearchGroceryItemsDto(
                groceryItem.Id,
                groceryItem.Name,
                groceryItem.Description,
                priceLog.Price,
                groceryItem.ImageUrl,
                groceryItem.Barcode,
                priceLog.LogDate,
                groceryItem.MeasureUnit,
                store.Id,
                store.Name);

        if (!string.IsNullOrEmpty(queryParams.Name))
            query = query.Where(g => g.Name.Contains(queryParams.Name));
        if (queryParams.StoreId <= 0)
            query = query.Where(g => g.StoreId == queryParams.StoreId);

        return await query
            .AsNoTracking()
            .Skip(queryParams.PageSize * queryParams.Page)
            .Take(queryParams.PageSize)
            .ToListAsync(ct);
    }

    public override async Task<GroceryItem?> GetByIdAsync(long id, CancellationToken ct)
    {
        var query = from g in _context.GroceryItems
            join p in _context.PriceLogs on g.Id equals p.GroceryItemId
            join s in _context.Stores on p.StoreId equals s.Id
            where g.Id == id
            select new { g, p, s };

        var result = await query.FirstOrDefaultAsync(ct);
        return result?.g;
    }

    public async Task<GroceryItem?> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, long storeId,
        CancellationToken ct)
    {
        var query =
            from g in _context.GroceryItems
            join p in _context.PriceLogs on g.Id equals p.GroceryItemId
            join s in _context.Stores on p.StoreId equals s.Id
            where s.Id == storeId
            select g;

        query = groceryItem.Barcode == "SEM GTIN"
            ? query.Where(g => g.Name == groceryItem.Name)
            : query.Where(g => g.Barcode == groceryItem.Barcode);

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task InsertPriceLog(PriceLog? priceLog, CancellationToken ct)
    {
        await _context.PriceLogs.AddAsync(priceLog, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<PriceLog?> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct)
    {
        return await _context.PriceLogs
            .Where(p => p.GroceryItemId == groceryItemId)
            .OrderByDescending(p => p.LogDate)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<StoreWithItems> GetByStoreAsync(long storeId, CancellationToken ct)
    {
        var query = from g in _context.GroceryItems
            join p in _context.PriceLogs on g.Id equals p.GroceryItemId
            join s in _context.Stores on p.StoreId equals s.Id
            where p.StoreId == storeId
            select new { g, p, s };

        var result = await query.ToListAsync(ct);
        return new StoreWithItems()
        {
            Store = result.First().s,
            Items = result.Select(r => r.g).ToList()
        };
    }

    public async Task<List<SearchGroceryItemsDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var query = await _context.GroceryItems
            .FromSqlRaw("SELECT * FROM GroceryItems ORDER BY RAND() LIMIT {0}", quantity)
            .AsNoTracking()
            .Include(g => g.PriceHistory)!
            .ThenInclude(p => p.Store)
            .ToListAsync(ct);

        
        return query.Select(g => new SearchGroceryItemsDto(
                g.Id,
                g.Name,
                g.Description,
                g.PriceHistory!.OrderBy(p => p.LogDate).Last().Price,
                g.ImageUrl,
                g.Barcode,
                g.PriceHistory!.OrderBy(p => p.LogDate).Last().LogDate,
                g.MeasureUnit,
                g.PriceHistory!.OrderBy(p => p.LogDate).Last().StoreId,
                g.PriceHistory!.OrderBy(p => p.LogDate).Last().Store.Name))
            .OrderBy(g => Guid.NewGuid())
            .ToList();
    }
    
    
    public void Dispose()
    {
    }
}