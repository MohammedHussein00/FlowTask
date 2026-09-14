namespace FlowTask.Domain.AccessControl;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("workspace_roles")]
public class WorkspaceRole
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    public virtual ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
    public virtual ICollection<WorkspaceUserRole> UserRoles { get; set; } = new List<WorkspaceUserRole>();
}

[Table("role_permissions")]
public class RolePermission
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceRoleId { get; set; }
    [Required][MaxLength(100)] public string PermissionName { get; set; } = string.Empty;
    public bool Allowed { get; set; } = true;

    [ForeignKey(nameof(WorkspaceRoleId))] public virtual WorkspaceRole Role { get; set; } = null!;
}

[Table("workspace_user_roles")]
public class WorkspaceUserRole
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int WorkspaceRoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))]     public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(UserId))]          public virtual AppUser User { get; set; } = null!;
    [ForeignKey(nameof(WorkspaceRoleId))] public virtual WorkspaceRole Role { get; set; } = null!;
}

[Table("permission_grants")]
public class PermissionGrant
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(100)] public string ResourceType { get; set; } = string.Empty;
    public int? ResourceId { get; set; }
    [Required][MaxLength(100)] public string Permission { get; set; } = string.Empty;
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}