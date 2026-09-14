namespace FlowTask.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public abstract class BaseEntityWithUpdate : BaseEntity
{
    public DateTime? UpdatedAt { get; set; }
}

public abstract class AuditableEntity : BaseEntityWithUpdate
{
    public int? CreatedById { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}