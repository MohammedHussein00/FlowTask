namespace FlowTask.Domain.Templates;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Identity;

[Table("space_templates")]
public class SpaceTemplate
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Configuration { get; set; }
    [MaxLength(500)] public string? IconUrl { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
}

[Table("list_templates")]
public class ListTemplate
{
    [Key] public int Id { get; set; }
    [Required] public int SpaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DefaultFields { get; set; }
    public string? DefaultStatuses { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(SpaceId))] public virtual Space Space { get; set; } = null!;
}