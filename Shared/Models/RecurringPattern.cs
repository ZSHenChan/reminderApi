using System.Text.Json.Serialization;
using Shared.Contracts.Enums;

namespace Shared.Models
{
  public class RecurringPattern
  {
    [JsonIgnore]
    public int Id { get; set; }
    public RecurringType RecurringType { get; set; }
    public int Interval { get; set; } = 1;
    public DateOnly? EndDate { get; set; }

    [JsonIgnore]
    public List<Reminder> Reminders { get; set; } = [];
  }
}
