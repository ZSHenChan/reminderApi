using personal_ai.Dtos.Reminder;
using personal_ai.Models;

namespace personal_ai.Contracts.Interfaces
{
  public interface IReminderRepository
  {
    Task<Reminder> AddAsync(Reminder reminder);
    Task<Reminder?> DeleteAsync(int id);
    Task<List<Reminder>> GetAllAsync();
    Task<Reminder?> GetByIdAsync(int id);
    Task<Reminder?> UpdateAsync(int id, CreateReminderRequestDto reminder);
  }
}
