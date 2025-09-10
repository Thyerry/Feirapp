using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Queries;
using Feirapp.Entities.Dtos;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository(BaseContext context) : IGroceryItemRepository, IDisposable
{
    public async Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsQuery queryParams, CancellationToken ct)
    {
        var query =
            from g in context.GroceryItems
            join p in context.PriceLogs on g.Id equals p.GroceryItemId into pg
            from p in pg.OrderByDescending(p => p.LogDate).Take(1)
            join s in context.Stores on p.StoreId equals s.Id
            where
                (string.IsNullOrEmpty(queryParams.Name) || g.Name.Contains(queryParams.Name)) &&
                (!queryParams.StoreId.HasValue || s.Id == queryParams.StoreId.Value)
            select new SearchGroceryItemsDto(g.Id, g.Name, g.Description, p.Price, g.ImageUrl, g.Barcode, p.LogDate, g.MeasureUnit, s.Id, s.Name);
        
        return await query
            .Skip(queryParams.PageSize * queryParams.Page)
            .Take(queryParams.PageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<GroceryItem> InsertAsync(GroceryItem entity, CancellationToken ct)
    {
        await context.GroceryItems.AddAsync(entity, ct);
        return entity;
    }

    public async Task InsertListAsync(List<GroceryItem> entities, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var groceryItem = await context.GroceryItems.FindAsync([new { Id = id }], ct);

        if (groceryItem == null)
        {
            throw new KeyNotFoundException($"Grocery item with ID {id} not found.");
        }

        context.GroceryItems.Remove(groceryItem);
    }
    
    public async Task<List<GroceryItemDto>> GetAllAsync(CancellationToken ct)
    {
    var query = 
        from gi in context.GroceryItems
        join pl in context.PriceLogs on gi.Id equals pl.GroceryItemId
        select new GroceryItemDto
        {
            Id = gi.Id,
            Name = gi.Name,
            Description = gi.Description,
            Barcode = gi.Barcode,
            CestCode = gi.CestCode,
            NcmCode = gi.NcmCode,
            ImageUrl = gi.ImageUrl,
            Brand = gi.Brand,
            MeasureUnit = gi.MeasureUnit,
            PriceHistory = new List<PriceLogDto> { pl }
        };
        
        return await query.AsNoTracking().ToListAsync(ct);
    }

    public async Task<GroceryItem?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var query = context.GroceryItems;
            // .Where(g => g.Id == id)
            // .Include(g => g.PriceHistory)!
            // .ThenInclude(p => p.Store);

        var sd = query.ToQueryString();
        
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<GroceryItem> AddIfNotExistsAsync(Func<GroceryItem, bool> predicate, GroceryItem entity, CancellationToken ct = default)
    {
        return await context.GroceryItems.AddIfNotExistsAsync(entity, predicate, ct);
    }

    public List<GroceryItem> GetByQuery(Func<GroceryItem, bool> predicate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<GroceryItem?> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, Guid storeId, CancellationToken ct)
    {
        var query = context.GroceryItems
            .Join(context.PriceLogs,
                g => g.Id,
                p => p.GroceryItemId,
                (g, p) => new { g, p })
            .Where(x => groceryItem.Barcode == "SEM GTIN" 
                ? x.g.Name == groceryItem.Name && x.p.StoreId == storeId
                : x.g.Barcode == groceryItem.Barcode)
            .Select(x => x.g);

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task InsertPriceLog(PriceLog priceLog, CancellationToken ct)
    {
        await context.PriceLogs.AddAsync(priceLog, ct);
    }

    public async Task<PriceLog?> GetLastPriceLogAsync(Guid groceryItemId, Guid storeId, CancellationToken ct)
    {
        return await context.PriceLogs
            .Where(p => p.GroceryItemId == groceryItemId && p.StoreId == storeId)
            .OrderByDescending(p => p.LogDate)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<StoreWithItems> GetByStoreAsync(Guid storeId, CancellationToken ct)
    {
        var store = await context.Stores.FindAsync([storeId], ct);
        
        if (store == null)
            throw new KeyNotFoundException("Store not found");
        
        var items = await (
            from g in context.GroceryItems
            join p in context.PriceLogs on g.Id equals p.GroceryItemId
            where p.StoreId == storeId
            select g).ToListAsync(ct);

        return new StoreWithItems { Store = store, Items = items };
    }

    public async Task<List<SearchGroceryItemsDto>> GetRandomGroceryItemsAsync(int quantity, CancellationToken ct)
    {
        var query = 
            from g in context.GroceryItems
            join p in context.PriceLogs on g.Id equals p.GroceryItemId
            join s in context.Stores on p.StoreId equals s.Id
            orderby BaseContext.Random()
            select new SearchGroceryItemsDto(g.Id, g.Name, g.Description, p.Price, g.ImageUrl, g.Barcode, p.LogDate, g.MeasureUnit, s.Id, s.Name);

        return await query.Take(quantity).ToListAsync(ct);
    }
    
    public async Task<bool> UpdateNameAndBrandAsync(Guid id, string name, string brand, CancellationToken ct)
    {
        var groceryItem = await context.GroceryItems.FindAsync([new { Id = id }], ct);
        if (groceryItem == null)
            return false;

        groceryItem.Name = name;
        groceryItem.Brand = brand;
        return true;
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}