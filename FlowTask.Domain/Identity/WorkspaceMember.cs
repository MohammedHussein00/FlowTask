namespace FlowTask.Domain.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("workspace_members")]
public class WorkspaceMember
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(50)] public string Role { get; set; } = "member";
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(UserId))]      public virtual AppUser User { get; set; } = null!;
}