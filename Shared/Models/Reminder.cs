using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Shared.Models;

public class Reminder : IReminder
{
  public int Id { get; set; }
  public DateOnly DueDate { get; set; }
  public TimeOnly DueTime { get; set; }
  public required string Title { get; set; }
  public required string Description { get; set; }
  public bool IsRecurring { get; set; } = false;
  public ReminderStatusType Status { get; set; }
  public PriorityLevelType Priority { get; set; }
  public RepeatFrequencyType RepeatFrequency { get; set; }
  public ReminderType ReminderType { get; set; }

  public Reminder()
  {
    Title = string.Empty;
    Description = string.Empty;
  }
}
