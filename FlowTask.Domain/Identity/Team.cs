namespace FlowTask.Domain.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("teams")]
public class Team
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string Handle { get; set; } = string.Empty;
    [MaxLength(500)] public string? AvatarUrl { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
}

[Table("team_members")]
public class TeamMember
{
    [Key] public int Id { get; set; }
    [Required] public int TeamId { get; set; }
    [Required] public int UserId { get; set; }

    [ForeignKey(nameof(TeamId))] public virtual Team Team { get; set; } = null!;
    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}