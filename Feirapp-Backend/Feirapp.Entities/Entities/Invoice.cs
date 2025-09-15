using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("invoices")]
public class Invoice
{
    [Column("id")]
    public Guid Id { get; set; }
    [Column("code")]
    public string Code { get; set; }
    [Column("url")]
    public string Url { get; set; }
    [Column("scan_date")]
    public DateTime ScanDate { get; set; }
    [Column("user_id")]
    public Guid UserId { get; set; }
}