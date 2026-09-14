namespace FlowTask.Domain.Productivity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("time_entries")]
public class TimeEntry
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int UserId { get; set; }
    public string? Description { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public DateTime EndTime { get; set; }
    [Required] public int Duration { get; set; }

    [ForeignKey(nameof(TaskId))] public virtual Tasks.Task Task { get; set; } = null!;
    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}

[Table("goals")]
public class Goal
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public int? OwnerId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    [MaxLength(7)] public string Color { get; set; } = "#3498DB";

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(OwnerId))]     public virtual AppUser Owner { get; set; } = null!;
    public virtual ICollection<KeyResult> KeyResults { get; set; } = new List<KeyResult>();
}

[Table("key_results")]
public class KeyResult
{
    [Key] public int Id { get; set; }
    [Required] public int GoalId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    [MaxLength(50)] public string ResultType { get; set; } = "numeric";
    public decimal? TargetValue { get; set; }
    public decimal CurrentValue { get; set; } = 0;
    [Required] public int? OwnerId { get; set; }

    [ForeignKey(nameof(GoalId))]  public virtual Goal Goal { get; set; } = null!;
    [ForeignKey(nameof(OwnerId))] public virtual AppUser Owner { get; set; } = null!;
}