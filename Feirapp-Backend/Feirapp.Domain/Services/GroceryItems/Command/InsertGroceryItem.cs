﻿namespace Feirapp.Domain.Services.GroceryItems.Command;

public record InsertGroceryItem(
    string Name,
    decimal Price,
    string MeasureUnit,
    string Barcode,
    string ProductCode,
    string? Brand,
    List<string>? AltNames,
    DateTime PurchaseDate,
    string NcmCode,
    string? CestCode
);