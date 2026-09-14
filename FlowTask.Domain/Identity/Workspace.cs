namespace FlowTask.Domain.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.AccessControl;
using FlowTask.Domain.Ai;
using FlowTask.Domain.Apps;
using FlowTask.Domain.Automations;
using FlowTask.Domain.Collaboration;
using FlowTask.Domain.Customization;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Productivity;
using FlowTask.Domain.Recurring;
using FlowTask.Domain.Reports;
using FlowTask.Domain.Security;
using FlowTask.Domain.SystemOps;
using FlowTask.Domain.Tasks;
using FlowTask.Domain.Templates;

[Table("workspaces")]
public class Workspace
{
    [Key] public int Id { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required] public int OwnerId { get; set; }
    [MaxLength(500)] public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(OwnerId))]
    public virtual AppUser Owner { get; set; } = null!;

    public virtual ICollection<WorkspaceMember> Members { get; set; } = new List<WorkspaceMember>();
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    public virtual ICollection<Space> Spaces { get; set; } = new List<Space>();
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public virtual ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public virtual ICollection<View> Views { get; set; } = new List<View>();
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual ICollection<Webhook> Webhooks { get; set; } = new List<Webhook>();
    public virtual ICollection<Integration> Integrations { get; set; } = new List<Integration>();
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();
    public virtual ICollection<Automation> Automations { get; set; } = new List<Automation>();
    public virtual ICollection<AppInstallation> AppInstallations { get; set; } = new List<AppInstallation>();
    public virtual ICollection<WorkspaceRole> Roles { get; set; } = new List<WorkspaceRole>();
    public virtual ICollection<WorkspaceUserRole> UserRoles { get; set; } = new List<WorkspaceUserRole>();
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    public virtual ICollection<Dashboard> Dashboards { get; set; } = new List<Dashboard>();
    public virtual ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    public virtual ICollection<AuditEvent> AuditEvents { get; set; } = new List<AuditEvent>();
    public virtual ICollection<ScheduledJob> ScheduledJobs { get; set; } = new List<ScheduledJob>();
    public virtual ICollection<SpaceTemplate> SpaceTemplates { get; set; } = new List<SpaceTemplate>();
    public virtual ICollection<PriorityLevel> PriorityLevels { get; set; } = new List<PriorityLevel>();
    public virtual ICollection<AiAgent> AiAgents { get; set; } = new List<AiAgent>();
    public virtual ICollection<AiModelConfig> AiModelConfigs { get; set; } = new List<AiModelConfig>();
    public virtual ICollection<AiSkill> AiSkills { get; set; } = new List<AiSkill>();
    public virtual ICollection<VectorEmbedding> VectorEmbeddings { get; set; } = new List<VectorEmbedding>();
    public virtual ICollection<AiKnowledgeBase> AiKnowledgeBase { get; set; } = new List<AiKnowledgeBase>();
    public virtual ICollection<AiPromptResponse> AiPromptResponses { get; set; } = new List<AiPromptResponse>();
    public virtual ICollection<AiModelUsage> AiModelUsage { get; set; } = new List<AiModelUsage>();
}