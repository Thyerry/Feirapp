namespace Feirapp.Entities.Entities;

public class Cest
{
    public string Code { get; set; }
    public string? Segment { get; set; }
    public string? Description { get; set; }
    public string? NcmCode { get; set; }
    public Ncm Ncm { get; set; }
    public ICollection<GroceryItem> GroceryItems { get; set; }
}