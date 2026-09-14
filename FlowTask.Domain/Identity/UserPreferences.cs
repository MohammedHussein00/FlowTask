namespace FlowTask.Domain.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("user_preferences")]
public class UserPreferences
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [MaxLength(50)] public string Theme { get; set; } = "light";
    [MaxLength(10)] public string Language { get; set; } = "en";
    public bool NotificationsEnabled { get; set; } = true;

    [ForeignKey(nameof(UserId))]
    public virtual AppUser User { get; set; } = null!;
}