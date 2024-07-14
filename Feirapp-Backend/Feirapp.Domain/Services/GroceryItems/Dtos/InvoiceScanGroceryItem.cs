﻿namespace Feirapp.Domain.Services.GroceryItems.Dtos;

public record InvoiceScanGroceryItem(
    string Name,
    decimal Price,
    string MeasureUnit,
    string Barcode,
    DateTime PurchaseDate,
    string NcmCode,
    string CestCode
)
{
    public decimal Quantity { get; set; }
};