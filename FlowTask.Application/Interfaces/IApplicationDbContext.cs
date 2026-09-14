namespace FlowTask.Application.Interfaces;

using Microsoft.EntityFrameworkCore;
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

public interface IApplicationDbContext
{
    // Identity & Access
    DbSet<Workspace>       Workspaces       { get; }
    DbSet<WorkspaceMember> WorkspaceMembers { get; }
    DbSet<Team>            Teams            { get; }
    DbSet<TeamMember>      TeamMembers      { get; }

    // Hierarchy
    DbSet<Space>       Spaces       { get; }
    DbSet<SpaceMember> SpaceMembers { get; }
    DbSet<Folder>      Folders      { get; }
    DbSet<List>        Lists        { get; }

    // Tasks
    DbSet<Domain.Tasks.Task> Tasks             { get; }
    DbSet<Status>            Statuses          { get; }
    DbSet<TaskAssignee>      TaskAssignees     { get; }
    DbSet<TaskWatcher>       TaskWatchers      { get; }
    DbSet<Dependency>        Dependencies      { get; }
    DbSet<TaskRelationship>  TaskRelationships { get; }
    DbSet<PriorityLevel>     PriorityLevels    { get; }
    DbSet<EffortEstimate>    EffortEstimates   { get; }

    // Collaboration
    DbSet<Comment>         Comments         { get; }
    DbSet<Attachment>      Attachments      { get; }
    DbSet<Tag>             Tags             { get; }
    DbSet<TaskTag>         TaskTags         { get; }
    DbSet<Checklist>       Checklists       { get; }
    DbSet<ChecklistItem>   ChecklistItems   { get; }
    DbSet<CommentReaction> CommentReactions { get; }
    DbSet<CommentThread>   CommentThreads   { get; }

    // Customization
    DbSet<CustomField>      CustomFields      { get; }
    DbSet<CustomFieldValue> CustomFieldValues { get; }

    // Productivity
    DbSet<TimeEntry> TimeEntries { get; }
    DbSet<Goal>      Goals       { get; }
    DbSet<KeyResult> KeyResults  { get; }

    // Sprints
    DbSet<Sprint>       Sprints       { get; }
    DbSet<SprintTask>   SprintTasks   { get; }
    DbSet<SprintReport> SprintReports { get; }

    // Security
    DbSet<AccessToken> AccessTokens { get; }
    DbSet<Invitation>  Invitations  { get; }
    DbSet<UserSession> UserSessions { get; }
    DbSet<AuditEvent>  AuditEvents  { get; }

    // System Ops
    DbSet<View>         Views         { get; }
    DbSet<ActivityLog>  ActivityLogs  { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Webhook>      Webhooks      { get; }
    DbSet<Integration>  Integrations  { get; }
    DbSet<Document>     Documents     { get; }
    DbSet<DocumentPage> DocumentPages { get; }
    DbSet<Template>     Templates     { get; }

    // Automations
    DbSet<Automation>          Automations          { get; }
    DbSet<AutomationAction>    AutomationActions    { get; }
    DbSet<AutomationTrigger>   AutomationTriggers   { get; }
    DbSet<AutomationExecution> AutomationExecutions { get; }
    DbSet<AutomationLog>       AutomationLogs       { get; }

    // AI
    DbSet<AiAgent>          AiAgents          { get; }
    DbSet<AgentPrompt>      AgentPrompts      { get; }
    DbSet<AgentAction>      AgentActions      { get; }
    DbSet<AiModelConfig>    AiModelConfigs    { get; }
    DbSet<AiAgentExecution> AiAgentExecutions { get; }
    DbSet<AiSkill>          AiSkills          { get; }
    DbSet<AiKnowledgeBase>  AiKnowledgeBases  { get; }
    DbSet<AiPromptResponse> AiPromptResponses { get; }
    DbSet<AiModelUsage>     AiModelUsage      { get; }
    DbSet<AiFeedbackRating> AiFeedbackRatings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}