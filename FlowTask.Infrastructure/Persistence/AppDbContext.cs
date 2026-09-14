namespace FlowTask.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlowTask.Application.Interfaces;
using FlowTask.Domain.AccessControl;
using FlowTask.Domain.Ai;
using FlowTask.Domain.Apps;
using FlowTask.Domain.Automations;
using FlowTask.Domain.Collaboration;
using FlowTask.Domain.Customization;
using FlowTask.Domain.FieldHistory;
using FlowTask.Domain.Hierarchy;
using FlowTask.Domain.Identity;
using FlowTask.Domain.Productivity;
using FlowTask.Domain.Recurring;
using FlowTask.Domain.Reports;
using FlowTask.Domain.Security;
using FlowTask.Domain.Sprints;
using FlowTask.Domain.SystemOps;
using FlowTask.Domain.Tasks;
using FlowTask.Domain.Templates;

public class AppDbContext : IdentityDbContext<
    AppUser, AppRole, int,
    AppUserClaim, AppUserRole, AppUserLogin,
    AppRoleClaim, AppUserToken>, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Identity & Access
    public DbSet<UserPreferences> UserPreferences { get; set; } = null!;
    public DbSet<Workspace> Workspaces { get; set; } = null!;
    public DbSet<WorkspaceMember> WorkspaceMembers { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<TeamMember> TeamMembers { get; set; } = null!;

    // Access Control
    public DbSet<WorkspaceRole> WorkspaceRoles { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<WorkspaceUserRole> WorkspaceUserRoles { get; set; } = null!;
    public DbSet<PermissionGrant> PermissionGrants { get; set; } = null!;

    // Hierarchy
    public DbSet<Space> Spaces { get; set; } = null!;
    public DbSet<SpaceMember> SpaceMembers { get; set; } = null!;
    public DbSet<Folder> Folders { get; set; } = null!;
    public DbSet<List> Lists { get; set; } = null!;

    // Tasks
    public DbSet<Domain.Tasks.Task> Tasks { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;
    public DbSet<TaskAssignee> TaskAssignees { get; set; } = null!;
    public DbSet<TaskWatcher> TaskWatchers { get; set; } = null!;
    public DbSet<Dependency> Dependencies { get; set; } = null!;
    public DbSet<TaskRelationship> TaskRelationships { get; set; } = null!;
    public DbSet<PriorityLevel> PriorityLevels { get; set; } = null!;
    public DbSet<EffortEstimate> EffortEstimates { get; set; } = null!;

    // Collaboration
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<TaskTag> TaskTags { get; set; } = null!;
    public DbSet<Checklist> Checklists { get; set; } = null!;
    public DbSet<ChecklistItem> ChecklistItems { get; set; } = null!;
    public DbSet<CommentReaction> CommentReactions { get; set; } = null!;
    public DbSet<CommentThread> CommentThreads { get; set; } = null!;

    // Customization
    public DbSet<CustomField> CustomFields { get; set; } = null!;
    public DbSet<CustomFieldValue> CustomFieldValues { get; set; } = null!;

    // Productivity
    public DbSet<TimeEntry> TimeEntries { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;
    public DbSet<KeyResult> KeyResults { get; set; } = null!;

    // System Ops
    public DbSet<View> Views { get; set; } = null!;
    public DbSet<ActivityLog> ActivityLogs { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Webhook> Webhooks { get; set; } = null!;
    public DbSet<Integration> Integrations { get; set; } = null!;
    public DbSet<Document> Documents { get; set; } = null!;
    public DbSet<DocumentPage> DocumentPages { get; set; } = null!;
    public DbSet<Template> Templates { get; set; } = null!;

    // Automations
    public DbSet<Automation> Automations { get; set; } = null!;
    public DbSet<AutomationTrigger> AutomationTriggers { get; set; } = null!;
    public DbSet<AutomationAction> AutomationActions { get; set; } = null!;
    public DbSet<AutomationExecution> AutomationExecutions { get; set; } = null!;
    public DbSet<AutomationLog> AutomationLogs { get; set; } = null!;

    // Apps
    public DbSet<App> Apps { get; set; } = null!;
    public DbSet<AppInstallation> AppInstallations { get; set; } = null!;
    public DbSet<AppPermission> AppPermissions { get; set; } = null!;
    public DbSet<AppWebhook> AppWebhooks { get; set; } = null!;

    // Sprints
    public DbSet<Sprint> Sprints { get; set; } = null!;
    public DbSet<SprintTask> SprintTasks { get; set; } = null!;
    public DbSet<SprintReport> SprintReports { get; set; } = null!;

    // Reports
    public DbSet<Report> Reports { get; set; } = null!;
    public DbSet<Dashboard> Dashboards { get; set; } = null!;
    public DbSet<DashboardWidget> DashboardWidgets { get; set; } = null!;

    // Field History
    public DbSet<FieldHistory> FieldHistories { get; set; } = null!;
    public DbSet<TaskVersion> TaskVersions { get; set; } = null!;
    public DbSet<ActivitySnapshot> ActivitySnapshots { get; set; } = null!;

    // Security
    public DbSet<AccessToken> AccessTokens { get; set; } = null!;
    public DbSet<Invitation> Invitations { get; set; } = null!;
    public DbSet<UserSession> UserSessions { get; set; } = null!;
    public DbSet<AuditEvent> AuditEvents { get; set; } = null!;

    // Recurring
    public DbSet<RecurringTask> RecurringTasks { get; set; } = null!;
    public DbSet<RecurringTaskInstance> RecurringTaskInstances { get; set; } = null!;
    public DbSet<ScheduledJob> ScheduledJobs { get; set; } = null!;

    // Templates
    public DbSet<SpaceTemplate> SpaceTemplates { get; set; } = null!;
    public DbSet<ListTemplate> ListTemplates { get; set; } = null!;

    // AI/LLM
    public DbSet<AiAgent> AiAgents { get; set; } = null!;
    public DbSet<AgentPrompt> AgentPrompts { get; set; } = null!;
    public DbSet<AgentAction> AgentActions { get; set; } = null!;
    public DbSet<AiModelConfig> AiModelConfigs { get; set; } = null!;
    public DbSet<AiAgentExecution> AiAgentExecutions { get; set; } = null!;
    public DbSet<AiSkill> AiSkills { get; set; } = null!;
    public DbSet<VectorEmbedding> VectorEmbeddings { get; set; } = null!;
    public DbSet<AiKnowledgeBase> AiKnowledgeBases { get; set; } = null!;
    public DbSet<AiPromptResponse> AiPromptResponses { get; set; } = null!;
    public DbSet<AiModelUsage> AiModelUsage { get; set; } = null!;
    public DbSet<AiFeedbackRating> AiFeedbackRatings { get; set; } = null!;

protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // ── Rename Identity tables ─────────────────────────────
    builder.Entity<AppUser>().ToTable("users");
    builder.Entity<AppRole>().ToTable("system_roles");
    builder.Entity<AppUserRole>().ToTable("system_user_roles");
    builder.Entity<AppUserClaim>().ToTable("user_claims");
    builder.Entity<AppUserLogin>().ToTable("user_logins");
    builder.Entity<AppUserToken>().ToTable("user_tokens");
    builder.Entity<AppRoleClaim>().ToTable("system_role_claims");

    // ── Unique Indexes ─────────────────────────────────────
    builder.Entity<WorkspaceMember>().HasIndex(e => new { e.WorkspaceId, e.UserId }).IsUnique();
    builder.Entity<TeamMember>().HasIndex(e => new { e.TeamId, e.UserId }).IsUnique();
    builder.Entity<SpaceMember>().HasIndex(e => new { e.SpaceId, e.UserId }).IsUnique();
    builder.Entity<TaskAssignee>().HasIndex(e => new { e.TaskId, e.UserId }).IsUnique();
    builder.Entity<TaskWatcher>().HasIndex(e => new { e.TaskId, e.UserId }).IsUnique();
    builder.Entity<Dependency>().HasIndex(e => new { e.TaskId, e.DependsOnTaskId }).IsUnique();
    builder.Entity<TaskRelationship>().HasIndex(e => new { e.SourceTaskId, e.TargetTaskId }).IsUnique();
    builder.Entity<TaskTag>().HasIndex(e => new { e.TaskId, e.TagId }).IsUnique();
    builder.Entity<CustomFieldValue>().HasIndex(e => new { e.CustomFieldId, e.TaskId }).IsUnique();
    builder.Entity<Tag>().HasIndex(e => new { e.WorkspaceId, e.Name }).IsUnique();
    builder.Entity<CustomField>().HasIndex(e => new { e.WorkspaceId, e.Name }).IsUnique();
    builder.Entity<Integration>().HasIndex(e => new { e.WorkspaceId, e.ServiceName }).IsUnique();
    builder.Entity<CommentReaction>().HasIndex(e => new { e.CommentId, e.UserId, e.Emoji }).IsUnique();
    builder.Entity<SprintTask>().HasIndex(e => new { e.SprintId, e.TaskId }).IsUnique();
    builder.Entity<WorkspaceRole>().HasIndex(e => new { e.WorkspaceId, e.Name }).IsUnique();
    builder.Entity<RolePermission>().HasIndex(e => new { e.WorkspaceRoleId, e.PermissionName }).IsUnique();
    builder.Entity<WorkspaceUserRole>().HasIndex(e => new { e.WorkspaceId, e.UserId, e.WorkspaceRoleId }).IsUnique();
    builder.Entity<ActivitySnapshot>().HasIndex(e => new { e.WorkspaceId, e.SnapshotDate }).IsUnique();
    builder.Entity<RecurringTaskInstance>().HasIndex(e => new { e.RecurringTaskId, e.SequenceNumber }).IsUnique();
    builder.Entity<TaskVersion>().HasIndex(e => new { e.TaskId, e.VersionNumber }).IsUnique();
    builder.Entity<PriorityLevel>().HasIndex(e => new { e.WorkspaceId, e.Name }).IsUnique();
    builder.Entity<AppInstallation>().HasIndex(e => new { e.AppId, e.WorkspaceId }).IsUnique();
    builder.Entity<AiModelConfig>().HasIndex(e => new { e.WorkspaceId, e.ModelName }).IsUnique();
    builder.Entity<AiSkill>().HasIndex(e => new { e.WorkspaceId, e.Name }).IsUnique();
    builder.Entity<AiModelUsage>().HasIndex(e => new { e.WorkspaceId, e.ModelName, e.UsageDate }).IsUnique();

    // ── Self-Referencing ───────────────────────────────────
    builder.Entity<Domain.Tasks.Task>()
        .HasOne(t => t.Parent)
        .WithMany(t => t.Children)
        .HasForeignKey(t => t.ParentId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<DocumentPage>()
        .HasOne(p => p.ParentPage)
        .WithMany(p => p.Children)
        .HasForeignKey(p => p.ParentPageId)
        .OnDelete(DeleteBehavior.Restrict);

    // ── AgentAction self-ref: NoAction (SetNull rejected by SQL Server) ──
    builder.Entity<AgentAction>()
        .HasOne(a => a.FallbackAction)
        .WithMany()
        .HasForeignKey(a => a.FallbackActionId)
        .OnDelete(DeleteBehavior.NoAction);

    // ── Dependency ─────────────────────────────────────────
    builder.Entity<Dependency>()
        .HasOne(d => d.Task)
        .WithMany(t => t.Dependencies)
        .HasForeignKey(d => d.TaskId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<Dependency>()
        .HasOne(d => d.DependsOnTask)
        .WithMany(t => t.BlockingDependencies)
        .HasForeignKey(d => d.DependsOnTaskId)
        .OnDelete(DeleteBehavior.Restrict);

    // ── TaskRelationship ───────────────────────────────────
    builder.Entity<TaskRelationship>()
        .HasOne(r => r.SourceTask)
        .WithMany(t => t.SourceRelationships)
        .HasForeignKey(r => r.SourceTaskId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<TaskRelationship>()
        .HasOne(r => r.TargetTask)
        .WithMany(t => t.TargetRelationships)
        .HasForeignKey(r => r.TargetTaskId)
        .OnDelete(DeleteBehavior.Restrict);

    // ── CommentThread ──────────────────────────────────────
    builder.Entity<CommentThread>()
        .HasOne(t => t.ReplyToComment)
        .WithMany()
        .HasForeignKey(t => t.ReplyToCommentId)
        .OnDelete(DeleteBehavior.NoAction);

    // ── Decimal Precision ──────────────────────────────────
    builder.Entity<AiModelConfig>().Property(p => p.Temperature).HasColumnType("decimal(3,2)");
    builder.Entity<AiModelConfig>().Property(p => p.TopP).HasColumnType("decimal(3,2)");
    builder.Entity<KeyResult>().Property(p => p.TargetValue).HasColumnType("decimal(12,2)");
    builder.Entity<KeyResult>().Property(p => p.CurrentValue).HasColumnType("decimal(12,2)");
    builder.Entity<SprintReport>().Property(p => p.Velocity).HasColumnType("decimal(10,2)");
    builder.Entity<EffortEstimate>().Property(p => p.EstimatedHours).HasColumnType("decimal(10,2)");
    builder.Entity<EffortEstimate>().Property(p => p.ActualHours).HasColumnType("decimal(10,2)");
    builder.Entity<AiKnowledgeBase>().Property(p => p.RelevanceScore).HasColumnType("decimal(3,2)");
    builder.Entity<AiAgentExecution>().Property(p => p.CostUsd).HasColumnType("decimal(10,6)");
    builder.Entity<AiModelUsage>().Property(p => p.CostUsd).HasColumnType("decimal(12,6)");

    // ── Global: Restrict all remaining Cascade behaviors ───
    // Must be LAST so all entity types are fully registered
    foreach (var fk in builder.Model.GetEntityTypes()
        .SelectMany(e => e.GetForeignKeys())
        .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade))
    {
        fk.DeleteBehavior = DeleteBehavior.Restrict;
    }
}
}