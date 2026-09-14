namespace FlowTask.Domain.FieldHistory;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("field_history")]
public class FieldHistory
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required][MaxLength(100)] public string FieldName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    [Required] public int ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TaskId))]    public virtual Tasks.Task Task { get; set; } = null!;
    [ForeignKey(nameof(ChangedBy))] public virtual AppUser ChangedByUser { get; set; } = null!;
}

[Table("task_versions")]
public class TaskVersion
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int VersionNumber { get; set; }
    [Required] public string Snapshot { get; set; } = string.Empty;
    [Required] public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TaskId))]    public virtual Tasks.Task Task { get; set; } = null!;
    [ForeignKey(nameof(CreatedBy))] public virtual AppUser CreatedByUser { get; set; } = null!;
}

[Table("activity_snapshots")]
public class ActivitySnapshot
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required] public DateTime SnapshotDate { get; set; }
    public string? ActivitySummary { get; set; }
    public string? Metrics { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Identity.Workspace Workspace { get; set; } = null!;
}