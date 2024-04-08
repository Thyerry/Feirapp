namespace Feirapp.Entities.Entities;

public class Ncm
{
    public string? Code { get; set; }
    public string? Description { get; set; }
    public ICollection<Cest> Cests { get; set; }
}