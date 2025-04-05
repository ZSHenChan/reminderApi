using System;
using personal_ai.Contracts.Enums;

namespace personal_ai.Contracts.Interfaces
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
