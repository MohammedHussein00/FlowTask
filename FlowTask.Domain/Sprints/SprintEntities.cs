namespace FlowTask.Domain.Sprints;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Hierarchy;

[Table("sprints")]
public class Sprint
{
    [Key] public int Id { get; set; }
    [Required] public int ListId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public DateTime StartDate { get; set; }
    [Required] public DateTime EndDate { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "planned";
    public string? Goal { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ListId))] public virtual List List { get; set; } = null!;
    public virtual ICollection<SprintTask> SprintTasks { get; set; } = new List<SprintTask>();
    public virtual ICollection<SprintReport> Reports { get; set; } = new List<SprintReport>();
}

[Table("sprint_tasks")]
public class SprintTask
{
    [Key] public int Id { get; set; }
    [Required] public int SprintId { get; set; }
    [Required] public int TaskId { get; set; }
    public int? StoryPoints { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(SprintId))] public virtual Sprint Sprint { get; set; } = null!;
    [ForeignKey(nameof(TaskId))]   public virtual Tasks.Task Task { get; set; } = null!;
}

[Table("sprint_reports")]
public class SprintReport
{
    [Key] public int Id { get; set; }
    [Required] public int SprintId { get; set; }
    public int? TotalTasks { get; set; }
    public int? CompletedTasks { get; set; }
    public int? TotalPoints { get; set; }
    public int? CompletedPoints { get; set; }
    public decimal? Velocity { get; set; }
    public string? BurndownData { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(SprintId))] public virtual Sprint Sprint { get; set; } = null!;
}