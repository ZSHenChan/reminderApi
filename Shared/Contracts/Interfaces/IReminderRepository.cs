using Shared.Dtos.Reminder;
using Shared.Models;
using Shared.Utils;

namespace Shared.Contracts.Interfaces
{
  public interface IReminderRepository
  {
    Task<Reminder[]> AddAsync(Reminder[] reminder);
    Task<List<Reminder>> GetAllAsync(QueryObject queryObject, string UserId);
    Task<Reminder?> GetByIdAsync(int id, string UserId);
    Task<Reminder?> UpdateAsync(int id, CreateReminderRequestDto reminder);
    Task<Reminder?> DeleteAsync(int id);
  }
}
