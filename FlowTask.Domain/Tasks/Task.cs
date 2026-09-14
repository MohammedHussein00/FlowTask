namespace FlowTask.Domain.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Collaboration;
using FlowTask.Domain.Customization;
using FlowTask.Domain.FieldHistory;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Identity;
using FlowTask.Domain.Productivity;
using FlowTask.Domain.Recurring;
using FlowTask.Domain.Sprints;

[Table("tasks")]
public class Task
{
    [Key] public int Id { get; set; }
    [Required] public int ListId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? StatusId { get; set; }
    [MaxLength(50)] public string Priority { get; set; } = "normal";
    [Required] public int CreatorId { get; set; }
    public int? ParentId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DateClosed { get; set; }
    public int? TimeEstimate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ListId))]   public virtual List List { get; set; } = null!;
    [ForeignKey(nameof(StatusId))] public virtual Status? Status { get; set; }
    [ForeignKey(nameof(CreatorId))]public virtual AppUser Creator { get; set; } = null!;
    [ForeignKey(nameof(ParentId))] public virtual Task? Parent { get; set; }

    public virtual ICollection<Task> Children { get; set; } = new List<Task>();
    public virtual ICollection<TaskAssignee> Assignees { get; set; } = new List<TaskAssignee>();
    public virtual ICollection<TaskWatcher> Watchers { get; set; } = new List<TaskWatcher>();
    public virtual ICollection<Dependency> Dependencies { get; set; } = new List<Dependency>();
    public virtual ICollection<Dependency> BlockingDependencies { get; set; } = new List<Dependency>();
    public virtual ICollection<TaskRelationship> SourceRelationships { get; set; } = new List<TaskRelationship>();
    public virtual ICollection<TaskRelationship> TargetRelationships { get; set; } = new List<TaskRelationship>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public virtual ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    public virtual ICollection<Checklist> Checklists { get; set; } = new List<Checklist>();
    public virtual ICollection<CustomFieldValue> CustomFieldValues { get; set; } = new List<CustomFieldValue>();
    public virtual ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public virtual ICollection<FieldHistory> FieldHistories { get; set; } = new List<FieldHistory>();
    public virtual ICollection<TaskVersion> TaskVersions { get; set; } = new List<TaskVersion>();
    public virtual ICollection<EffortEstimate> EffortEstimates { get; set; } = new List<EffortEstimate>();
    public virtual ICollection<SprintTask> SprintTasks { get; set; } = new List<SprintTask>();
    public virtual ICollection<RecurringTaskInstance> RecurringInstances { get; set; } = new List<RecurringTaskInstance>();
}

[Table("statuses")]
public class Status
{
    [Key] public int Id { get; set; }
    public int? SpaceId { get; set; }
    public int? ListId { get; set; }
    [Required][MaxLength(50)] public string Name { get; set; } = string.Empty;
    [MaxLength(7)] public string Color { get; set; } = "#95A5A6";
    public int OrderIndex { get; set; }
    [MaxLength(50)] public string? StatusType { get; set; }

    [ForeignKey(nameof(SpaceId))] public virtual Hierarchy.Space? Space { get; set; }
    [ForeignKey(nameof(ListId))]  public virtual Hierarchy.List? List { get; set; }
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}

[Table("task_assignees")]
public class TaskAssignee
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int UserId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TaskId))] public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}

[Table("task_watchers")]
public class TaskWatcher
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int UserId { get; set; }

    [ForeignKey(nameof(TaskId))] public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
}

[Table("dependencies")]
public class Dependency
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int DependsOnTaskId { get; set; }
    [MaxLength(50)] public string DependencyType { get; set; } = "blocks";

    [ForeignKey(nameof(TaskId))]          public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(DependsOnTaskId))] public virtual Task DependsOnTask { get; set; } = null!;
}

[Table("task_relationships")]
public class TaskRelationship
{
    [Key] public int Id { get; set; }
    [Required] public int SourceTaskId { get; set; }
    [Required] public int TargetTaskId { get; set; }
    [MaxLength(50)] public string RelationshipType { get; set; } = "relates_to";

    [ForeignKey(nameof(SourceTaskId))] public virtual Task SourceTask { get; set; } = null!;
    [ForeignKey(nameof(TargetTaskId))] public virtual Task TargetTask { get; set; } = null!;
}

[Table("priority_levels")]
public class PriorityLevel
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required] public int Value { get; set; }
    [MaxLength(7)] public string? Color { get; set; }
    public string? Description { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Identity.Workspace Workspace { get; set; } = null!;
}

[Table("effort_estimates")]
public class EffortEstimate
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public int? EffortPoints { get; set; }
    [MaxLength(50)] public string? ConfidenceLevel { get; set; }
    [Required] public int EstimatedBy { get; set; }
    public DateTime EstimatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TaskId))]     public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(EstimatedBy))]public virtual AppUser EstimatedByUser { get; set; } = null!;
}