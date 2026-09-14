namespace FlowTask.Domain.Automations;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Common;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Identity;

[Table("automations")]
public class Automation : BaseEntityWithUpdate
{
    [Required] public int WorkspaceId { get; set; }
    public int? SpaceId { get; set; }
    public int? ListId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    [Required][MaxLength(100)] public string TriggerType { get; set; } = string.Empty;
    public string? ConditionLogic { get; set; }
    [Required] public int? CreatedBy { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(SpaceId))]     public virtual Space? Space { get; set; }
    [ForeignKey(nameof(ListId))]      public virtual List? List { get; set; }
    [ForeignKey(nameof(CreatedBy))]   public virtual AppUser CreatedByUser { get; set; } = null!;

    public virtual ICollection<AutomationTrigger> Triggers { get; set; } = new List<AutomationTrigger>();
    public virtual ICollection<AutomationAction> Actions { get; set; } = new List<AutomationAction>();
    public virtual ICollection<AutomationExecution> Executions { get; set; } = new List<AutomationExecution>();
    public virtual ICollection<AutomationLog> Logs { get; set; } = new List<AutomationLog>();
}

[Table("automation_triggers")]
public class AutomationTrigger
{
    [Key] public int Id { get; set; }
    [Required] public int AutomationId { get; set; }
    [Required][MaxLength(100)] public string TriggerEvent { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string EntityType { get; set; } = string.Empty;
    public string? Conditions { get; set; }
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(AutomationId))] public virtual Automation Automation { get; set; } = null!;
}

[Table("automation_actions")]
public class AutomationAction
{
    [Key] public int Id { get; set; }
    [Required] public int AutomationId { get; set; }
    [Required][MaxLength(100)] public string ActionType { get; set; } = string.Empty;
    public string? ActionConfig { get; set; }
    public int? OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    [ForeignKey(nameof(AutomationId))] public virtual Automation Automation { get; set; } = null!;
}

[Table("automation_executions")]
public class AutomationExecution
{
    [Key] public int Id { get; set; }
    [Required] public int AutomationId { get; set; }
    [MaxLength(100)] public string? TriggeredBy { get; set; }
    public int? EntityId { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "success";
    public DateTime ExecutionTime { get; set; } = DateTime.UtcNow;
    public int? DurationMs { get; set; }

    [ForeignKey(nameof(AutomationId))] public virtual Automation Automation { get; set; } = null!;
    public virtual ICollection<AutomationLog> Logs { get; set; } = new List<AutomationLog>();
}

[Table("automation_logs")]
public class AutomationLog
{
    [Key] public int Id { get; set; }
    [Required] public int AutomationId { get; set; }
    public int? ExecutionId { get; set; }
    [MaxLength(50)] public string? LogLevel { get; set; }
    public string? Message { get; set; }
    public string? ErrorDetails { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(AutomationId))] public virtual Automation Automation { get; set; } = null!;
    [ForeignKey(nameof(ExecutionId))]  public virtual AutomationExecution? Execution { get; set; }
}