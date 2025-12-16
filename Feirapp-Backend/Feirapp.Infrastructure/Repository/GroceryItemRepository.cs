using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Methods.GetGroceryItemById;
using Feirapp.Domain.Services.GroceryItems.Methods.SearchGroceryItems;
using Feirapp.Entities.Dtos;
using Feirapp.Entities.Entities;
using Feirapp.Entities.Enums;
using Feirapp.Infrastructure.Configuration;
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
            where (string.IsNullOrEmpty(requestParams.Name) || EF.Functions.ILike(g.Name,$"%{requestParams.Name}%")) 
               && (!requestParams.StoreId.HasValue || s.Id == requestParams.StoreId.Value)
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
            .AsNoTracking()
            .OrderByDescending(g => g.LastUpdate)
            .Skip(requestParams.PageSize * requestParams.PageIndex)
            .Take(requestParams.PageSize)
            .ToListAsync(ct);
    }

    public async Task<GroceryItem> InsertAsync(GroceryItem entity, CancellationToken ct)
    {
        await context.GroceryItems.AddAsync(entity, ct);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var groceryItem = await context.GroceryItems.FirstOrDefaultAsync(g => g.Id == id, ct);
        
        if (groceryItem == null)
        {
            throw new KeyNotFoundException("Grocery item with not found.");
        }
        
        var priceLogs = context.PriceLogs.Where(p => p.GroceryItemId == groceryItem.Id);
        
        if (priceLogs.Any())
        {
            context.PriceLogs.RemoveRange(priceLogs);
        }
        
        context.GroceryItems.Remove(groceryItem);
    }

    public async Task<GetGroceryItemByIdDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var query =
            from g in context.GroceryItems
            where g.Id == id
            select new GetGroceryItemByIdDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Barcode = g.Barcode,
                Brand = g.Brand,
                CestCode = g.CestCode,
                ImageUrl = g.ImageUrl,
                MeasureUnit = g.MeasureUnit,
                NcmCode = g.NcmCode,
                PriceHistory = (
                    from pl in context.PriceLogs
                    join s in context.Stores on pl.StoreId equals s.Id
                    where pl.GroceryItemId == g.Id
                    orderby pl.LogDate descending
                    select new GetGroceryItemByIdPriceLogDto
                    {
                        LogDate = pl.LogDate,
                        Price = pl.Price,
                        Store = new GetGroceryItemByIdStoreDto()
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Cep = s.Cep,
                            CityName = s.CityName,
                            Cnpj = s.Cnpj,
                            Neighborhood = s.Neighborhood,
                            State = s.State.StringValue(),
                            Street = s.Street,
                            StreetNumber = s.StreetNumber,
                            AltNames = s.AltNames
                        }
                    }
                ).ToList()
            };
        
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<GroceryItem?> CheckIfGroceryItemExistsAsync(string barcode, string productCode, Guid storeId, CancellationToken ct)
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
                select g
            ).ToListAsync(ct);

        return new StoreWithItems
        {
            Store = store,
            Items = items
        };
    }
    
    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}