namespace FlowTask.Domain.Hierarchy;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;
using FlowTask.Domain.Tasks;
using FlowTask.Domain.Templates;

[Table("spaces")]
public class Space
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    public bool IsPrivate { get; set; }
    [MaxLength(7)] public string Color { get; set; } = "#5B9BD5";
    [MaxLength(500)] public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    public virtual ICollection<SpaceMember> Members { get; set; } = new List<SpaceMember>();
    public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();
    public virtual ICollection<List> Lists { get; set; } = new List<List>();
    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();
    public virtual ICollection<ListTemplate> ListTemplates { get; set; } = new List<ListTemplate>();
}

[Table("space_members")]
public class SpaceMember
{
    [Key] public int Id { get; set; }
    [Required] public int SpaceId { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(50)] public string AccessLevel { get; set; } = "member";

    [ForeignKey(nameof(SpaceId))] public virtual Space Space { get; set; } = null!;
    [ForeignKey(nameof(UserId))]  public virtual AppUser User { get; set; } = null!;
}

[Table("folders")]
public class Folder
{
    [Key] public int Id { get; set; }
    [Required] public int SpaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    public bool IsHidden { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(SpaceId))] public virtual Space Space { get; set; } = null!;
    public virtual ICollection<List> Lists { get; set; } = new List<List>();
}

[Table("lists")]
public class List
{
    [Key] public int Id { get; set; }
    public int? FolderId { get; set; }
    [Required] public int SpaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? StartDate { get; set; }
    [MaxLength(50)] public string Priority { get; set; } = "normal";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(FolderId))] public virtual Folder? Folder { get; set; }
    [ForeignKey(nameof(SpaceId))]  public virtual Space Space { get; set; } = null!;
    public virtual ICollection<FlowTask.Domain.Tasks.Task> Tasks { get; set; } = new List<FlowTask.Domain.Tasks.Task>();
    public virtual ICollection<Status> Statuses { get; set; } = new List<Status>();
    public virtual ICollection<Automations.Automation> Automations { get; set; } = new List<Automations.Automation>();
    public virtual ICollection<Sprints.Sprint> Sprints { get; set; } = new List<Sprints.Sprint>();
    public virtual ICollection<Recurring.RecurringTask> RecurringTasks { get; set; } = new List<Recurring.RecurringTask>();
}