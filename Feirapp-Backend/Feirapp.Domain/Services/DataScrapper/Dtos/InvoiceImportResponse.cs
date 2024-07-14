using Feirapp.Domain.Services.GroceryItems.Command;
using Feirapp.Domain.Services.GroceryItems.Dtos;

namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record InvoiceImportResponse(InvoiceScanStore Store, List<InvoiceScanGroceryItem> Items);