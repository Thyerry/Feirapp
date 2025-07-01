using Feirapp.Domain.Services.GroceryItems.Dtos;

namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record InvoiceImportResponse(string InvoiceCode, InvoiceScanStore? Store, List<InvoiceScanGroceryItem> Items);