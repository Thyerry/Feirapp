using System.ComponentModel.DataAnnotations.Schema;

namespace Feirapp.Entities.Entities;

[Table("Invoices")]
public class Invoice
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string Url { get; set; }
    public DateTime ScanDate { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}