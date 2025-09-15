using Feirapp.Domain.Services.DataScrapper.Methods.InvoiceScan;

namespace Feirapp.Domain.Services.DataScrapper.Interfaces;

public interface IInvoiceReaderService
{
    Task<InvoiceImportResponse> InvoiceImportAsync(string invoiceCode, bool isInsert, CancellationToken ct);
}