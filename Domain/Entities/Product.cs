using Domain.Common;

namespace Domain.Entities;

public class Product : AuditableBaseEntity
{
    public string Barcode { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public decimal Rate { get; set; }
}