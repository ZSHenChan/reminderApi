using System.Text.Json.Serialization;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Shared.Models;

public class Reminder : IReminder
{
  public int Id { get; set; }
  public DateOnly? DueDate { get; set; }
  public TimeOnly? DueTime { get; set; }
  public required string Title { get; set; } = string.Empty;
  public required string Description { get; set; } = string.Empty;
  public ReminderStatusType Status { get; set; }
  public PriorityLevelType Priority { get; set; }
  public ReminderType ReminderType { get; set; }
  public int? RecurringPatternId { get; set; }
  public RecurringPattern? RecurringPattern { get; set; }

  // User
  [JsonIgnore]
  public string AppUserId { get; set; } = null!;

  [JsonIgnore]
  public AppUser AppUser { get; set; } = null!;
}
