namespace FlowTask.Domain.Customization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlowTask.Domain.Identity;

[Table("custom_fields")]
public class CustomField
{
    [Key] public int Id { get; set; }
    [Required] public int WorkspaceId { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Required][MaxLength(50)]  public string FieldType { get; set; } = string.Empty;
    public string? Configuration { get; set; }

    [ForeignKey(nameof(WorkspaceId))] public virtual Workspace Workspace { get; set; } = null!;
    public virtual ICollection<CustomFieldValue> Values { get; set; } = new List<CustomFieldValue>();
}

[Table("custom_field_values")]
public class CustomFieldValue
{
    [Key] public int Id { get; set; }
    [Required] public int CustomFieldId { get; set; }
    [Required] public int TaskId { get; set; }
    public string? Value { get; set; }

    [ForeignKey(nameof(CustomFieldId))] public virtual CustomField CustomField { get; set; } = null!;
    [ForeignKey(nameof(TaskId))]        public virtual Tasks.Task Task { get; set; } = null!;
}