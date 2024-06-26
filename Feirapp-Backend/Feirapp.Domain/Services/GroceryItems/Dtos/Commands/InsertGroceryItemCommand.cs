using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Entities.Enums;

namespace Feirapp.Domain.Services.GroceryItems.Dtos.Commands;

public record InsertGroceryItemCommand(
    List<InvoiceGroceryItem> Items,
    InvoiceStore Store
);