namespace FlowTask.Domain.Security;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("access_tokens")]
public class AccessToken
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(255)] public string TokenHash { get; set; } = string.Empty;
    [MaxLength(100)] public string? Name { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}

[Table("invitations")]
public class Invitation
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][EmailAddress][MaxLength(255)] public string Email { get; set; } = string.Empty;
    [Required] public int InvitedBy { get; set; }
    [Required][MaxLength(50)] public string Role { get; set; } = string.Empty;
    [MaxLength(50)] public string Status { get; set; } = "pending";
    [MaxLength(255)] public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(InvitedBy))]   public virtual AppUser InvitedByUser { get; set; } = null!;
}

[Table("user_sessions")]
public class UserSession
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [MaxLength(255)] public string? SessionToken { get; set; }
    [MaxLength(45)]  public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }

    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}

[Table("audit_events")]
public class AuditEvent
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    public int? UserId { get; set; }
    [Required][MaxLength(100)] public string EventType { get; set; } = string.Empty;
    [MaxLength(100)] public string? ResourceType { get; set; }
    public int? ResourceId { get; set; }
    public string? Details { get; set; }
    [MaxLength(45)] public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(UserId))]      public virtual AppUser? User { get; set; }
}