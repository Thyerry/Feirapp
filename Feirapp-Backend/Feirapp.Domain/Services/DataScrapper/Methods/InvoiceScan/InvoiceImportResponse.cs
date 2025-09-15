namespace Feirapp.Domain.Services.DataScrapper.Methods.InvoiceScan;

public record InvoiceImportResponse(string InvoiceCode, InvoiceImportStore? Store, List<InvoiceImportGroceryItem> Items);