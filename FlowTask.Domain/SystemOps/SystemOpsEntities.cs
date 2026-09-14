namespace FlowTask.Domain.SystemOps;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;
using FlowTask.Domain.Hierarchy;

[Table("views")]
public class View
{
    [Key] public int Id { get; set; }
    public int? WorkspaceId { get; set; }
    public int? SpaceId { get; set; }
    public int? FolderId { get; set; }
    public int? ListId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string ViewType { get; set; } = "table";
    public string? QuerySettings { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace? Workspace { get; set; }
    [ForeignKey(nameof(SpaceId))]     public virtual Space? Space { get; set; }
    [ForeignKey(nameof(FolderId))]    public virtual Folder? Folder { get; set; }
    [ForeignKey(nameof(ListId))]      public virtual List? List { get; set; }
}

[Table("activity_logs")]
public class ActivityLog
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(100)] public string Action { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string EntityType { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(UserId))]      public virtual AppUser User { get; set; } = null!;
}

[Table("notifications")]
public class Notification
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(255)] public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public bool IsRead { get; set; }
    [MaxLength(50)] public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}

[Table("webhooks")]
public class Webhook
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(500)] public string Url { get; set; } = string.Empty;
    [MaxLength(255)] public string? Events { get; set; }
    public bool IsActive { get; set; } = true;
    [Required][MaxLength(255)] public string Secret { get; set; } = string.Empty;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
}

[Table("integrations")]
public class Integration
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string ServiceName { get; set; } = string.Empty;
    [Required] public string Credentials { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
}

[Table("documents")]
public class Document
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    [Required] public int CreatorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(CreatorId))]   public virtual AppUser Creator { get; set; } = null!;
    public virtual ICollection<DocumentPage> Pages { get; set; } = new List<DocumentPage>();
}

[Table("document_pages")]
public class DocumentPage
{
    [Key] public int Id { get; set; }
    [Required] public int DocumentId { get; set; }
    public int? ParentPageId { get; set; }
    [Required][MaxLength(255)] public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public int OrderIndex { get; set; }

    [ForeignKey(nameof(DocumentId))]   public virtual Document Document { get; set; } = null!;
    [ForeignKey(nameof(ParentPageId))] public virtual DocumentPage? ParentPage { get; set; }
    public virtual ICollection<DocumentPage> Children { get; set; } = new List<DocumentPage>();
}

[Table("templates")]
public class Template
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string TemplateType { get; set; } = "task";
    [Required] public string Content { get; set; } = string.Empty;
    [Required] public int CreatedBy { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(CreatedBy))]   public virtual AppUser CreatedByUser { get; set; } = null!;
}