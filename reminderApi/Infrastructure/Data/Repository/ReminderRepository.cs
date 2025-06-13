using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Interfaces;
using Shared.Dtos.Reminder;
using Shared.Models;
using Shared.Utils;

namespace reminderApi.Infrastructure.Data.Repository
{
  public class ReminderRepository : IReminderRepository
  {
    private readonly AppDBContext _context;

    public ReminderRepository(AppDBContext context)
    {
      _context = context;
    }

    public async Task<List<Reminder>> GetAllAsync(QueryObject queryObject, string UserId)
    {
      var reminders = _context.Reminders.AsQueryable();
      reminders = reminders.Where(r => r.AppUserId == UserId);

      if (!string.IsNullOrWhiteSpace(queryObject.Title))
      {
        reminders = reminders.Where(r => r.Title.Contains(queryObject.Title));
      }

      if (!string.IsNullOrWhiteSpace(queryObject.Description))
      {
        reminders = reminders.Where(r => r.Description.Contains(queryObject.Description));
      }

      if (queryObject.RemiderType.HasValue)
      {
        reminders = reminders.Where(r => r.ReminderType == queryObject.RemiderType);
      }

      if (queryObject.ReminderStatus.HasValue)
      {
        reminders = reminders.Where(r => r.Status == queryObject.ReminderStatus);
      }

      reminders = reminders.OrderBy(r => r.DueDate).ThenBy(r => r.DueTime);

      int skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
      reminders = reminders.Skip(skipNumber).Take(queryObject.PageSize);

      return await reminders.ToListAsync();
    }

    public async Task<Reminder?> GetByIdAsync(int id, string UserId)
    {
      return await _context.Reminders.FindAsync(id);
    }

    public async Task<Reminder[]> AddAsync(Reminder[] reminders)
    {
      foreach (Reminder reminder in reminders)
      {
        await _context.Reminders.AddAsync(reminder);
      }
      await _context.SaveChangesAsync();
      return [];
    }

    public async Task<Reminder?> UpdateAsync(int id, CreateReminderRequestDto reminder)
    {
      var existingReminder = await _context.Reminders.FindAsync(id);
      if (existingReminder == null)
      {
        return null;
      }
      ReminderEnumConverter.UpdateReminderSql(existingReminder, reminder);
      _context.Reminders.Update(existingReminder);
      // return await _context.SaveChangesAsync().ContinueWith(_ => existingReminder);
      await _context.SaveChangesAsync();
      return existingReminder;
    }

    public async Task<Reminder?> DeleteAsync(int id, string userId)
    {
      Reminder? existingReminder = await _context.Reminders.FindAsync(id);
      if (existingReminder == null || existingReminder.AppUserId != userId)
      {
        return null;
      }
      _context.Reminders.Remove(existingReminder);
      await _context.SaveChangesAsync();
      return existingReminder;
    }
  }
}
