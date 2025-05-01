using Shared.Models;

namespace Shared.Contracts.Interfaces
{
  public interface IRedisContext
  {
    public void StoreReminders(List<Reminder> reminders, string userId);
    public void DeleteReminders(List<int> reminderIdList, string userId);
    public List<Reminder> GetAllReminders(string userId);
  }
}
