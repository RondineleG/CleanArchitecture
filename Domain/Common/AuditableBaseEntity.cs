using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class AuditableBaseEntity<T> : BaseEntity<T>, IAuditableEntity
{
    public DateTime Created { get; set; }

    [StringLength(40)]
    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    [StringLength(40)]
    public string LastModifiedBy { get; set; }
}

public abstract class AuditableBaseEntity : AuditableBaseEntity<int>
{
}