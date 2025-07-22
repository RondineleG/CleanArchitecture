using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class BaseEntity<T> : IEntity<T>
{
    [Key]
    public virtual T Id { get; set; }
}