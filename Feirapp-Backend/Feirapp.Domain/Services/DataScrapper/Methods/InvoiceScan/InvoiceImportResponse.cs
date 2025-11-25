namespace Feirapp.Domain.Services.DataScrapper.Methods.InvoiceScan;

public class InvoiceImportResponse
{
    public string InvoiceCode { get; set; }
    public InvoiceImportStore? Store { get; set; }
    public List<InvoiceImportGroceryItem> Items { get; set; }
}