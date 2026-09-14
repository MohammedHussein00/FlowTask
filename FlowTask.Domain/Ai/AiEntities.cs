namespace FlowTask.Domain.Ai;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Common;
using FlowTask.Domain.Identity;

[Table("ai_agents")]
public class AiAgent : BaseEntityWithUpdate
{
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required][MaxLength(50)] public string AgentType { get; set; } = "super_agent";
    [Required] public string Purpose { get; set; } = string.Empty;
    public string? TriggerConditions { get; set; }
    public bool Enabled { get; set; } = true;
    [Required] public int? CreatedBy { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(CreatedBy))]   public virtual AppUser CreatedByUser { get; set; } = null!;
    public virtual ICollection<AgentPrompt> Prompts { get; set; } = new List<AgentPrompt>();
    public virtual ICollection<AgentAction> Actions { get; set; } = new List<AgentAction>();
    public virtual ICollection<AiAgentExecution> Executions { get; set; } = new List<AiAgentExecution>();
}

[Table("agent_prompts")]
public class AgentPrompt
{
    [Key] public int Id { get; set; }
    [Required] public int AiAgentId { get; set; }
    [Required][MaxLength(100)] public string PromptType { get; set; } = "system";
    [Required] public string PromptText { get; set; } = string.Empty;
    public int Version { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(AiAgentId))] public virtual AiAgent AiAgent { get; set; } = null!;
}

[Table("agent_actions")]
public class AgentAction
{
    [Key] public int Id { get; set; }
    [Required] public int AiAgentId { get; set; }
    [Required][MaxLength(100)] public string ActionType { get; set; } = string.Empty;
    public string? ActionConfig { get; set; }
    public int? OrderIndex { get; set; }
    [MaxLength(50)] public string? RetryPolicy { get; set; }
    public int? FallbackActionId { get; set; }

    [ForeignKey(nameof(AiAgentId))]      public virtual AiAgent AiAgent { get; set; } = null!;
    [ForeignKey(nameof(FallbackActionId))]public virtual AgentAction? FallbackAction { get; set; }
}

[Table("ai_model_configs")]
public class AiModelConfig
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string ModelName { get; set; } = string.Empty;
    [Required][MaxLength(100)] public string ModelProvider { get; set; } = "openai";
    [MaxLength(255)] public string? ApiKeyHash { get; set; }
    public decimal Temperature { get; set; } = 0.7m;
    public int MaxTokens { get; set; } = 2000;
    public decimal? TopP { get; set; }
    public bool IsDefault { get; set; }
    public bool Enabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
}

[Table("ai_agent_executions")]
public class AiAgentExecution
{
    [Key] public int Id { get; set; }
    [Required] public int AiAgentId { get; set; }
    [MaxLength(100)] public string? TriggeredBy { get; set; }
    public int? TriggeredByUserId { get; set; }
    [MaxLength(100)] public string? ModelUsed { get; set; }
    public string? InputPrompt { get; set; }
    public string? ModelResponse { get; set; }
    public string? ActionsExecuted { get; set; }
    [Required][MaxLength(50)] public string ExecutionStatus { get; set; } = "success";
    public string? ErrorMessage { get; set; }
    public DateTime ExecutionTime { get; set; } = DateTime.UtcNow;
    public int? DurationMs { get; set; }
    public int TokensInput { get; set; }
    public int TokensOutput { get; set; }
    public int TokensTotal { get; set; }
    public decimal CostUsd { get; set; }

    [ForeignKey(nameof(AiAgentId))]         public virtual AiAgent AiAgent { get; set; } = null!;
    [ForeignKey(nameof(TriggeredByUserId))]  public virtual AppUser? TriggeredByUser { get; set; }
    public virtual ICollection<AiFeedbackRating> FeedbackRatings { get; set; } = new List<AiFeedbackRating>();
}

[Table("ai_skills")]
public class AiSkill
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required] public string PromptTemplate { get; set; } = string.Empty;
    [MaxLength(100)] public string? Category { get; set; }
    public string? UseCases { get; set; }
    public bool IsPublic { get; set; }
    [Required] public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(CreatedBy))]   public virtual AppUser CreatedByUser { get; set; } = null!;
}

[Table("vector_embeddings")]
public class VectorEmbedding
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string SourceType { get; set; } = string.Empty;
    public int? SourceId { get; set; }
    [MaxLength(255)] public string? SourceTitle { get; set; }
    [Required] public string TextChunk { get; set; } = string.Empty;
    [Required] public string EmbeddingVector { get; set; } = string.Empty;
    [MaxLength(100)] public string? EmbeddingModel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
}

[Table("ai_knowledge_base")]
public class AiKnowledgeBase
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Category { get; set; } = string.Empty;
    [Required][MaxLength(255)] public string Title { get; set; } = string.Empty;
    [Required] public string Content { get; set; } = string.Empty;
    [MaxLength(500)] public string? SourceUrl { get; set; }
    public decimal? RelevanceScore { get; set; }
    public int? LastUpdatedById { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))]     public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(LastUpdatedById))] public virtual AppUser? LastUpdatedBy { get; set; }
}

[Table("ai_prompt_responses")]
public class AiPromptResponse
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required] public int? UserId { get; set; }
    [Required] public string PromptInput { get; set; } = string.Empty;
    [Required] public string ModelResponse { get; set; } = string.Empty;
    [Required][MaxLength(100)] public string ModelUsed { get; set; } = string.Empty;
    [MaxLength(50)] public string? UserFeedback { get; set; }
    public string? FeedbackComment { get; set; }
    [MaxLength(100)] public string? ContextType { get; set; }
    public int? ContextId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    [ForeignKey(nameof(UserId))]      public virtual AppUser User { get; set; } = null!;
    public virtual ICollection<AiFeedbackRating> FeedbackRatings { get; set; } = new List<AiFeedbackRating>();
}

[Table("ai_model_usage")]
public class AiModelUsage
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string ModelName { get; set; } = string.Empty;
    [Required] public DateTime UsageDate { get; set; }
    public int RequestCount { get; set; } = 1;
    public int TokensInput { get; set; }
    public int TokensOutput { get; set; }
    public int TotalTokens { get; set; }
    public decimal CostUsd { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
}

[Table("ai_feedback_ratings")]
public class AiFeedbackRating
{
    [Key] public int Id { get; set; }
    public int? AiAgentExecutionId { get; set; }
    public int? AiPromptResponseId { get; set; }
    [Required] public int? UserId { get; set; }
    public int? Rating { get; set; }
    public int? AccuracyRating { get; set; }
    public int? RelevanceRating { get; set; }
    public int? HelpfulnessRating { get; set; }
    public string? Comments { get; set; }
    public string? ImprovementSuggestions { get; set; }
    public DateTime RatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(AiAgentExecutionId))]  public virtual AiAgentExecution? AiAgentExecution { get; set; }
    [ForeignKey(nameof(AiPromptResponseId))]  public virtual AiPromptResponse? AiPromptResponse { get; set; }
    [ForeignKey(nameof(UserId))]              public virtual AppUser User { get; set; } = null!;
}