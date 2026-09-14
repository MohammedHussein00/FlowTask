namespace FlowTask.Domain.Recurring;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Identity;

[Table("recurring_tasks")]
public class RecurringTask
{
    [Key] public int Id { get; set; }
    [Required] public int ListId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required][MaxLength(100)] public string RecurrencePattern { get; set; } = string.Empty;
    public string? RecurrenceConfig { get; set; }
    [Required] public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ListId))] public virtual List List { get; set; } = null!;
    public virtual ICollection<RecurringTaskInstance> Instances { get; set; } = new List<RecurringTaskInstance>();
}

[Table("recurring_task_instances")]
public class RecurringTaskInstance
{
    [Key] public int Id { get; set; }
    [Required] public int RecurringTaskId { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public DateTime DueDate { get; set; }
    public int? SequenceNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(RecurringTaskId))] public virtual RecurringTask RecurringTask { get; set; } = null!;
    [ForeignKey(nameof(TaskId))]          public virtual Tasks.Task Task { get; set; } = null!;
}

[Table("scheduled_jobs")]
public class ScheduledJob
{
    [Key] public int Id { get; set; }
    [Required][MaxLength(100)] public string JobType { get; set; } = string.Empty;
    public int? WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Schedule { get; set; } = string.Empty;
    public DateTime? LastRun { get; set; }
    public DateTime? NextRun { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "active";
    public string? Config { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace? Workspace { get; set; }
}