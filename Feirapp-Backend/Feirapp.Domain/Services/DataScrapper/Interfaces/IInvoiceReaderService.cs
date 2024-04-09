using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Entities.Entities;

namespace Feirapp.Domain.Services.DataScrapper.Interfaces;

public interface IInvoiceReaderService
{
    Task<List<InvoiceGroceryItem>> InvoiceDataScrapperAsync(string invoiceCode, CancellationToken ct);
}