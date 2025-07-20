using System;

namespace Domain.Common;

public interface IAuditableEntity<T> : IAuditableEntity
{
    public T Id { get; set; }
}

public interface IAuditableEntity
{
    public DateTime Created { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string LastModifiedBy { get; set; }
}