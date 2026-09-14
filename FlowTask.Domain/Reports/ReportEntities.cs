namespace FlowTask.Domain.Reports;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("reports")]
public class Report
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required][MaxLength(100)] public string ReportType { get; set; } = string.Empty;
    public string? Filters { get; set; }
    [Required] public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(CreatedBy))]   public virtual AppUser CreatedByUser { get; set; } = null!;
}

[Table("dashboards")]
public class Dashboard
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public int CreatedBy { get; set; }
    public bool IsShared { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(CreatedBy))]   public virtual AppUser CreatedByUser { get; set; } = null!;
    public virtual ICollection<DashboardWidget> Widgets { get; set; } = new List<DashboardWidget>();
}

[Table("dashboard_widgets")]
public class DashboardWidget
{
    [Key] public int Id { get; set; }
    [Required] public int DashboardId { get; set; }
    [Required][MaxLength(100)] public string WidgetType { get; set; } = string.Empty;
    [Required][MaxLength(255)] public string Title { get; set; } = string.Empty;
    public string? DataSource { get; set; }
    [MaxLength(50)] public string? Position { get; set; }
    [MaxLength(50)] public string? Size { get; set; }

    [ForeignKey(nameof(DashboardId))] public virtual Dashboard Dashboard { get; set; } = null!;
}