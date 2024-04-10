namespace Feirapp.Domain.Services.DataScrapper.Dtos;

public record InvoiceImportResponse(InvoiceStore Store, List<InvoiceGroceryItem> Items);