using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using personal_ai.Contracts.Interfaces;
using personal_ai.Data;
using personal_ai.Dtos.Reminder;
using personal_ai.Models;
using personal_ai.Utils;

namespace personal_ai.Repository
{
  public class ReminderRepository : IReminderRepository
  {
    private readonly AppDBContext _context;

    public ReminderRepository(AppDBContext context)
    {
      _context = context;
    }

    public async Task<List<Reminder>> GetAllAsync()
    {
      try
      {
        return await _context.Reminders.ToListAsync();
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
