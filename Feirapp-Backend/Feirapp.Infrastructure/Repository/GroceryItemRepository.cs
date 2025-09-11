using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Entities.Dtos;
using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Repository;

public class GroceryItemRepository(BaseContext context) : IGroceryItemRepository, IDisposable
{
    public async Task<List<SearchGroceryItemsDto>> SearchGroceryItemsAsync(SearchGroceryItemsRequest requestParams, CancellationToken ct)
    {
        var query =
            from g in context.GroceryItems
            join p in context.PriceLogs on g.Id equals p.GroceryItemId into pg
            from p in pg.OrderByDescending(p => p.LogDate).Take(1)
            join s in context.Stores on p.StoreId equals s.Id
            where
                (string.IsNullOrEmpty(requestParams.Name) || g.Name.Contains(requestParams.Name)) &&
                (!requestParams.StoreId.HasValue || s.Id == requestParams.StoreId.Value)
            select new SearchGroceryItemsDto
            {
                Id= g.Id, 
                Name = g.Name,
                Description = g.Description,
                LastPrice = p.Price,
                ImageUrl = g.ImageUrl,
                Barcode = g.Barcode,
                LastUpdate = p.LogDate,
                MeasureUnit = g.MeasureUnit,
                StoreId = s.Id,
                StoreName = s.Name
            };
        
        return await query
            .Skip(requestParams.PageSize * requestParams.Page)
            .Take(requestParams.PageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<GroceryItem> InsertAsync(GroceryItem entity, CancellationToken ct)
    {
        await context.GroceryItems.AddAsync(entity, ct);
        return entity;
    }

    public async Task InsertListAsync(List<GroceryItem> groceryItems, CancellationToken ct)
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

    public async Task<GetGroceryItemByIdDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var query =
            from g in context.GroceryItems
            join pl in context.PriceLogs on g.Id equals pl.GroceryItemId
            join s in context.Stores on pl.StoreId equals s.Id
            select new GetGroceryItemByIdDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
            };
        
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

    public async Task<GroceryItem?> CheckIfGroceryItemExistsAsync(string barcode,
        string productCode, Guid storeId, CancellationToken ct)
    {
        var query = context.GroceryItems
            .Join(context.PriceLogs,
                g => g.Id,
                p => p.GroceryItemId,
                (g, p) => new { g, p })
            .Where(x => barcode == "SEM GTIN" 
                ? x.p.ProductCode == productCode && x.p.StoreId == storeId
                : x.g.Barcode == barcode)
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
             select new SearchGroceryItemsDto
             {
                 Id = g.Id,
                 Name = g.Name,
                 Description = g.Description,
                 LastPrice = p.Price,
                 ImageUrl = g.ImageUrl,
                 Barcode = g.Barcode,
                 LastUpdate = p.LogDate,
                 MeasureUnit = g.MeasureUnit,
                 StoreId = s.Id,
                 StoreName = s.Name
             };

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