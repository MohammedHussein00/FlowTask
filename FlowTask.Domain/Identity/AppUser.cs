namespace FlowTask.Domain.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using FlowTask.Domain.AccessControl;
using FlowTask.Domain.Ai;
using FlowTask.Domain.Apps;
using FlowTask.Domain.Automations;
using FlowTask.Domain.Collaboration;
using FlowTask.Domain.Customization;
using FlowTask.Domain.FieldHistory;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Productivity;
using FlowTask.Domain.Recurring;
using FlowTask.Domain.Reports;
using FlowTask.Domain.Security;
using FlowTask.Domain.Sprints;
using FlowTask.Domain.SystemOps;
using FlowTask.Domain.Tasks;

/// <summary>
/// Main user entity. Extends IdentityUser<int> to leverage ASP.NET Core Identity.
/// Provides: password hashing, 2FA, email confirmation, external logins, password reset, lockout.
/// </summary>
[Table("users")]
public class AppUser : IdentityUser<int>
{
    // Custom properties (NOT in IdentityUser)
    [MaxLength(100)]
    [Column("full_name")]
    public string? FullName { get; set; }

    [MaxLength(500)]
    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [MaxLength(100)]
    [Column("timezone")]
    public string Timezone { get; set; } = "UTC";

    [Column("created_at")]
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // ==========================================================
    // NAVIGATION PROPERTIES — Domain relationships
    // ==========================================================

    // Identity & Access
    public virtual UserPreferences? UserPreferences { get; set; }
    public virtual ICollection<WorkspaceMember> WorkspaceMemberships { get; set; } = new List<WorkspaceMember>();
    public virtual ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
    public virtual ICollection<SpaceMember> SpaceMemberships { get; set; } = new List<SpaceMember>();

    // Workspace ownership
    public virtual ICollection<Workspace> OwnedWorkspaces { get; set; } = new List<Workspace>();

    // Tasks
    public virtual ICollection<FlowTask.Domain.Tasks.Task> CreatedTasks { get; set; } = new List<FlowTask.Domain.Tasks.Task>();
    public virtual ICollection<TaskAssignee> TaskAssignments { get; set; } = new List<TaskAssignee>();
    public virtual ICollection<TaskWatcher> WatchedTasks { get; set; } = new List<TaskWatcher>();
    public virtual ICollection<ChecklistItem> AssignedChecklistItems { get; set; } = new List<ChecklistItem>();
    public virtual ICollection<EffortEstimate> EffortEstimates { get; set; } = new List<EffortEstimate>();

    // Collaboration
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Attachment> UploadedAttachments { get; set; } = new List<Attachment>();
    public virtual ICollection<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();

    // Productivity
    public virtual ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    public virtual ICollection<Goal> OwnedGoals { get; set; } = new List<Goal>();
    public virtual ICollection<KeyResult> OwnedKeyResults { get; set; } = new List<KeyResult>();

    // System Operations
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<Document> CreatedDocuments { get; set; } = new List<Document>();
    public virtual ICollection<Template> CreatedTemplates { get; set; } = new List<Template>();

    // Security
    public virtual ICollection<AccessToken> AccessTokens { get; set; } = new List<AccessToken>();
    public virtual ICollection<Invitation> SentInvitations { get; set; } = new List<Invitation>();
    public virtual ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    public virtual ICollection<AuditEvent> AuditEvents { get; set; } = new List<AuditEvent>();

    // Access Control (Workspace-level RBAC)
    public virtual ICollection<WorkspaceUserRole> WorkspaceUserRoles { get; set; } = new List<WorkspaceUserRole>();
    public virtual ICollection<PermissionGrant> PermissionGrants { get; set; } = new List<PermissionGrant>();

    // Automations
    public virtual ICollection<Automation> CreatedAutomations { get; set; } = new List<Automation>();

    // Apps
    public virtual ICollection<App> DevelopedApps { get; set; } = new List<App>();
    public virtual ICollection<AppInstallation> AppInstallations { get; set; } = new List<AppInstallation>();

    // Field History
    public virtual ICollection<FieldHistory> FieldHistories { get; set; } = new List<FieldHistory>();
    public virtual ICollection<TaskVersion> TaskVersions { get; set; } = new List<TaskVersion>();

    // Reports
    public virtual ICollection<Report> CreatedReports { get; set; } = new List<Report>();
    public virtual ICollection<Dashboard> CreatedDashboards { get; set; } = new List<Dashboard>();

    // AI/LLM
    public virtual ICollection<AiAgent> CreatedAiAgents { get; set; } = new List<AiAgent>();
    public virtual ICollection<AiSkill> CreatedAiSkills { get; set; } = new List<AiSkill>();
    public virtual ICollection<AiPromptResponse> AiPromptResponses { get; set; } = new List<AiPromptResponse>();
    public virtual ICollection<AiFeedbackRating> AiFeedbackRatings { get; set; } = new List<AiFeedbackRating>();
    public virtual ICollection<AiAgentExecution> TriggeredAiAgentExecutions { get; set; } = new List<AiAgentExecution>();
    public virtual ICollection<AiKnowledgeBase> UpdatedKnowledgeBases { get; set; } = new List<AiKnowledgeBase>();
}

