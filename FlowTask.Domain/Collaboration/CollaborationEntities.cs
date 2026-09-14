namespace FlowTask.Domain.Collaboration;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;
using FlowTask.Domain.Tasks;

[Table("comments")]
public class Comment
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public string Text { get; set; } = string.Empty;
    public bool Resolved { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TaskId))] public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(UserId))] public virtual AppUser User { get; set; } = null!;
    public virtual ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();
    public virtual CommentThread? Thread { get; set; }
}

[Table("attachments")]
public class Attachment
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int UploaderId { get; set; }
    [Required][MaxLength(255)] public string FileName { get; set; } = string.Empty;
    public int? FileSize { get; set; }
    [Required][MaxLength(500)] public string FileUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(TaskId))]    public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(UploaderId))]public virtual AppUser Uploader { get; set; } = null!;
}

[Table("tags")]
public class Tag
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    [MaxLength(7)] public string Color { get; set; } = "#E74C3C";

    [ForeignKey(nameof(WorkspaceId))] public virtual Identity.Workspace Workspace { get; set; } = null!;
    public virtual ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
}

[Table("task_tags")]
public class TaskTag
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required] public int TagId { get; set; }

    [ForeignKey(nameof(TaskId))] public virtual Task Task { get; set; } = null!;
    [ForeignKey(nameof(TagId))]  public virtual Tag Tag { get; set; } = null!;
}

[Table("checklists")]
public class Checklist
{
    [Key] public int Id { get; set; }
    [Required] public int TaskId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    public int OrderIndex { get; set; }

    [ForeignKey(nameof(TaskId))] public virtual Task Task { get; set; } = null!;
    public virtual ICollection<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();
}

[Table("checklist_items")]
public class ChecklistItem
{
    [Key] public int Id { get; set; }
    [Required] public int ChecklistId { get; set; }
    [Required][MaxLength(255)] public string Name { get; set; } = string.Empty;
    public bool IsResolved { get; set; }
    public int? AssigneeId { get; set; }
    public int OrderIndex { get; set; }

    [ForeignKey(nameof(ChecklistId))] public virtual Checklist Checklist { get; set; } = null!;
    [ForeignKey(nameof(AssigneeId))]  public virtual AppUser? Assignee { get; set; }
}

[Table("comment_reactions")]
public class CommentReaction
{
    [Key] public int Id { get; set; }
    [Required] public int CommentId { get; set; }
    [Required] public int UserId { get; set; }
    [Required][MaxLength(10)] public string Emoji { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(CommentId))] public virtual Comment Comment { get; set; } = null!;
    [ForeignKey(nameof(UserId))]    public virtual AppUser User { get; set; } = null!;
}

[Table("comment_threads")]
public class CommentThread
{
    [Key] public int Id { get; set; }
    [Required] public int CommentId { get; set; }
    public int? ReplyToCommentId { get; set; }
    public int? ThreadLevel { get; set; }

    [ForeignKey(nameof(CommentId))]       public virtual Comment Comment { get; set; } = null!;
    [ForeignKey(nameof(ReplyToCommentId))]public virtual Comment? ReplyToComment { get; set; }
}