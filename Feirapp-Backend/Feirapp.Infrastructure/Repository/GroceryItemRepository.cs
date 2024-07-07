using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Entities.Dtos;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;



public class GroceryItemRepository(BaseContext context)
    : BaseRepository<GroceryItem>(context), IGroceryItemRepository, IDisposable
{
    private readonly BaseContext _context = context;

    public async Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsQuery queryParams, CancellationToken ct)
    {
        var query = 
            from g in _context.GroceryItems join p in _context.PriceLogs on g.Id equals p.GroceryItemId into pg
            from p in pg.OrderByDescending(p => p.LogDate).Take(1) join s in _context.Stores on p.StoreId equals s.Id
            where 
                (string.IsNullOrEmpty(queryParams.Name) || g.Name.Contains(queryParams.Name))
                && (queryParams.StoreId <= 0 || s.Id == queryParams.StoreId)
            select new SearchGroceryItemsDto(g.Id, g.Name, g.Description, p.Price, g.ImageUrl, g.Barcode, p.LogDate, g.MeasureUnit, s.Id, s.Name);
        
        var uee = query.ToQueryString();

        return await query
            .Skip(queryParams.PageSize * queryParams.Page)
            .Take(queryParams.PageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }
    public override async Task<GroceryItem?> GetByIdAsync(long id, CancellationToken ct)
    {
        var query = _context.GroceryItems
            .Where(g => g.Id == id)
            .Include(g => g.PriceHistory)!
            .ThenInclude(p => p.Store);

        var result = await query.FirstOrDefaultAsync(ct);
        return result;
    }

    public async Task<(GroceryItem?, Store?)> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, long storeId,
        CancellationToken ct)
    {
        var item = await _context.GroceryItems.FirstOrDefaultAsync(g => g.Barcode == groceryItem.Barcode, ct);
        var store = await _context.Stores.FindAsync(storeId, ct);
        return (item, store);
    }

    public async Task InsertPriceLog(PriceLog? priceLog, CancellationToken ct)
    {
        await _context.PriceLogs.AddAsync(priceLog, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<PriceLog?> GetLastPriceLogAsync(long groceryItemId, long storeId, CancellationToken ct)
    {
        return await _context.PriceLogs
            .Where(p => p.GroceryItemId == groceryItemId && p.StoreId == storeId)
            .OrderByDescending(p => p.LogDate)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<StoreWithItems> GetByStoreAsync(long storeId, CancellationToken ct)
    {
        var items = await (from g in _context.GroceryItems
            join p in _context.PriceLogs on g.Id equals p.GroceryItemId
            where p.StoreId == storeId
            select g).ToListAsync(ct);

        var store = await _context.Stores.FindAsync(storeId, ct);
        return new StoreWithItems { Store = store, Items = items };
    }

    public async Task<List<SearchGroceryItemsDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var query = from g in _context.GroceryItems
            join p in _context.PriceLogs on g.Id equals p.GroceryItemId
            join s in _context.Stores on p.StoreId equals s.Id
            orderby BaseContext.Random()
            select new SearchGroceryItemsDto(g.Id, g.Name, g.Description, p.Price, g.ImageUrl, g.Barcode, p.LogDate, g.MeasureUnit, s.Id, s.Name);

        return await query.Take(quantity).ToListAsync(ct);
    }


    public void Dispose()
    {
    }
}