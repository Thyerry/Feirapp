using System.Data.SqlTypes;
using System.Runtime.Intrinsics.X86;
using Feirapp.Domain.Services.GroceryItems.Dtos;
using Feirapp.Domain.Services.GroceryItems.Dtos.Queries;
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

    public async Task<List<GroceryItemList>> ListGroceryItemsAsync(ListGroceryItemsQuery queryParams,
        CancellationToken ct)
    {
        var query = 
            from groceryItem in _context.GroceryItems
            join priceLog in _context.PriceLogs on groceryItem.Id equals priceLog.GroceryItemId
            join store in _context.Stores on priceLog.StoreId equals store.Id
            select new GroceryItemList(
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

    public async Task<GroceryItem?> CheckIfGroceryItemExistsAsync(GroceryItem groceryItem, long storeId, CancellationToken ct)
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

    public async Task InsertPriceLog(PriceLog priceLog, CancellationToken ct)
    {
        await _context.PriceLogs.AddAsync(priceLog, ct);
    }

    public async Task<PriceLog> GetLastPriceLogAsync(long groceryItemId, CancellationToken ct)
    {
        return await _context.PriceLogs
            .Where(p => p.GroceryItemId == groceryItemId)
            .OrderByDescending(p => p.LogDate)
            .FirstOrDefaultAsync(ct);
    }

    public void Dispose()
    {
    }
}