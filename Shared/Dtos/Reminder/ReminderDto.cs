using Shared.Contracts.Enums;
using Shared.Models;

namespace Shared.Dtos.Reminder
{
  public class ReminderDto
  {
    public int Id { get; set; }
    public DateOnly? DueDate { get; set; }
    public TimeOnly? DueTime { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public bool IsRecurring { get; set; } = false;
    public ReminderStatusType Status { get; set; }
    public PriorityLevelType Priority { get; set; }
    public RecurringPattern? RecurringPattern { get; set; }
    public ReminderType ReminderType { get; set; }
  }
}
