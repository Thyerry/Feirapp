using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("ncms")]
public class Ncm
{
    [Column("id")]
    public string Code { get; set; } = string.Empty;
    [Column("description")]
    public string? Description { get; set; }
    [Column("last_update")]
    public DateTime LastUpdate { get; set; }
}