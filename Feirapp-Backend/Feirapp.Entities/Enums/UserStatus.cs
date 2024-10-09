namespace Feirapp.Entities.Enums;

public enum UserStatus
{
    [StringValue("Empty")]
    Empty = 0,
    [StringValue("Active")]
    Active = 1,
    [StringValue("Inactive")]
    Inactive = 2,
    [StringValue("Blocked")]
    Blocked = 3
}