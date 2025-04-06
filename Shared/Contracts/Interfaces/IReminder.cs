using Shared.Contracts.Enums;

namespace Shared.Contracts.Interfaces
{
  public interface IReminder
  {
    int Id { get; set; }
    DateOnly DueDate { get; set; }
    TimeOnly DueTime { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    bool IsRecurring { get; set; }
    ReminderStatusType Status { get; set; }
    PriorityLevelType Priority { get; set; }
    RepeatFrequencyType RepeatFrequency { get; set; }
    ReminderType ReminderType { get; set; }
  }
}
