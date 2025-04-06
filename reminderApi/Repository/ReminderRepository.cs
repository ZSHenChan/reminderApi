using Microsoft.EntityFrameworkCore;
using reminderApi.Data;
using Shared.Contracts.Interfaces;
using Shared.Dtos.Reminder;
using Shared.Models;
using Shared.Utils;

namespace reminderApi.Repository
{
  public class ReminderRepository : IReminderRepository
  {
    private readonly AppDBContext _context;

    public ReminderRepository(AppDBContext context)
    {
      _context = context;
    }

    public async Task<List<Reminder>> GetAllAsync(QueryObject queryObject)
    {
      try
      {
        var reminders = _context.Reminders.AsQueryable();
        if (!string.IsNullOrWhiteSpace(queryObject.Title))
        {
          reminders = reminders.Where(r => r.Title.Contains(queryObject.Title));
        }

        if (!string.IsNullOrWhiteSpace(queryObject.Description))
        {
          reminders = reminders.Where(r => r.Description.Contains(queryObject.Description));
        }

        if (queryObject.IsRecurring.HasValue)
        {
          reminders = reminders.Where(r => r.IsRecurring == queryObject.IsRecurring);
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
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<Reminder?> GetByIdAsync(int id)
    {
      try
      {
        return await _context.Reminders.FindAsync(id);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<Reminder> AddAsync(Reminder reminder)
    {
      try
      {
        _context.Reminders.Add(reminder);
        return await _context.SaveChangesAsync().ContinueWith(_ => reminder);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<Reminder?> UpdateAsync(int id, CreateReminderRequestDto reminder)
    {
      try
      {
        var existingReminder = await _context.Reminders.FindAsync(id);
        if (existingReminder == null)
        {
          return null;
        }
        ReminderEnumConverter.UpdateReminderSql(existingReminder, reminder);
        _context.Reminders.Update(existingReminder);
        return await _context.SaveChangesAsync().ContinueWith(_ => existingReminder);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public async Task<Reminder?> DeleteAsync(int id)
    {
      try
      {
        Reminder? existingReminder = await _context.Reminders.FindAsync(id);
        if (existingReminder == null)
        {
          return null;
        }
        _context.Reminders.Remove(existingReminder);
        await _context.SaveChangesAsync();
        return existingReminder;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
