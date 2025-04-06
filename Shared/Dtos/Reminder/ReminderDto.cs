using Shared.Contracts.Enums;

namespace Shared.Dtos.Reminder
{
  public class ReminderDto
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
  }
}
