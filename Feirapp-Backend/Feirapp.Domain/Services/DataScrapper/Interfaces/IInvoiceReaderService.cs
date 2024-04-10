using Feirapp.Domain.Services.DataScrapper.Dtos;

namespace Feirapp.Domain.Services.DataScrapper.Interfaces;

public interface IInvoiceReaderService
{
    Task<InvoiceImportResponse> InvoiceDataScrapperAsync(string invoiceCode, CancellationToken ct);
}