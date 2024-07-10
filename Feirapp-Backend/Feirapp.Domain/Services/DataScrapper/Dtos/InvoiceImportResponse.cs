using Feirapp.Domain.Services.GroceryItems.Command;

namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record InvoiceImportResponse(InvoiceStore Store, List<InsertGroceryItem> Items);