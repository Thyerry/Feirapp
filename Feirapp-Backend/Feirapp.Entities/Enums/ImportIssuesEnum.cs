namespace Feirapp.Entities.Enums;

public enum ImportIssuesEnum
{
    [StringValue("No barcode and product code")]
    NoBarcodeAndProductCode = 1,
    [StringValue("Multiple items with the same barcode")]
    MultipleSameItemByKilo = 2,
    [StringValue("Price is zero or negative")]
    PriceIsZeroOrNegative = 3,
    [StringValue("Name is empty")]
    NameIsEmpty = 4,
    [StringValue("Quantity is zero or negative")]
    QuantityIsZeroOrNegative = 5,
    [StringValue("Measure unit is empty")]
    MeasureUnitIsEmpty = 6
}