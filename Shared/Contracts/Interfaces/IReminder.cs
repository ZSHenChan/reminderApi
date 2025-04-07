using Shared.Contracts.Enums;
using Shared.Models;

namespace Shared.Contracts.Interfaces
{
  public interface IReminder
  {
    int Id { get; set; }
    DateOnly? DueDate { get; set; }
    TimeOnly? DueTime { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    ReminderStatusType Status { get; set; }
    PriorityLevelType Priority { get; set; }
    ReminderType ReminderType { get; set; }
    int? RecurringPatternId { get; set; }
    RecurringPattern? RecurringPattern { get; set; }
    AppUser AppUser { get; set; }
  }
}
