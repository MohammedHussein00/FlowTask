namespace FlowTask.Domain.Apps;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("apps")]
public class App
{
    [Key] public int Id { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public int? DeveloperId { get; set; }
    public string? Description { get; set; }
    [MaxLength(500)] public string? IconUrl { get; set; }
    [MaxLength(500)] public string? DocumentationUrl { get; set; }
    [MaxLength(50)]  public string? Version { get; set; }
    public bool IsPublic { get; set; } = true;
    [MaxLength(100)] public string? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(DeveloperId))] public virtual AppUser? Developer { get; set; }
    public virtual ICollection<AppInstallation> Installations { get; set; } = new List<AppInstallation>();
    public virtual ICollection<AppPermission> Permissions { get; set; } = new List<AppPermission>();
}

[Table("app_installations")]
public class AppInstallation
{
    [Key] public int Id { get; set; }
    [Required] public int AppId { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required] public int InstalledBy { get; set; }
    public string? Configuration { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime InstalledAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(AppId))]       public virtual App App { get; set; } = null!;
    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(InstalledBy))] public virtual AppUser InstalledByUser { get; set; } = null!;
    public virtual ICollection<AppWebhook> Webhooks { get; set; } = new List<AppWebhook>();
}

[Table("app_permissions")]
public class AppPermission
{
    [Key] public int Id { get; set; }
    [Required] public int AppId { get; set; }
    [Required][MaxLength(100)] public string PermissionType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Required { get; set; }

    [ForeignKey(nameof(AppId))] public virtual App App { get; set; } = null!;
}

[Table("app_webhooks")]
public class AppWebhook
{
    [Key] public int Id { get; set; }
    [Required] public int AppInstallationId { get; set; }
    [Required][MaxLength(500)] public string WebhookUrl { get; set; } = string.Empty;
    [MaxLength(255)] public string? Events { get; set; }
    [MaxLength(255)] public string? Secret { get; set; }
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(AppInstallationId))] public virtual AppInstallation AppInstallation { get; set; } = null!;
}