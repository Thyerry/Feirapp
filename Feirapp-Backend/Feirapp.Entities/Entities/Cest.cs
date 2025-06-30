using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("cests")]
public class Cest
{
    [Column("code")]
    public string Code { get; set; }
    [Column("segment")]
    public string? Segment { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("ncm_code")]
    public string? NcmCode { get; set; }
    public Ncm Ncm { get; set; }
    public ICollection<GroceryItem> GroceryItems { get; set; }
}