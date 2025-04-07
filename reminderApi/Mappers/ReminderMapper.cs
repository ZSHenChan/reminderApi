using Shared.Contracts.Enums;
using Shared.Dtos.Reminder;
using Shared.Models;

namespace reminderApi.Mappers
{
  public static class ReminderMapper
  {
    public static ReminderDto ToReminderDto(this Reminder reminderModel)
    {
      try
      {
        ReminderDto reminderDtoResult = new()
        {
          Id = reminderModel.Id,
          DueDate = reminderModel.DueDate,
          DueTime = reminderModel.DueTime,
          Title = reminderModel.Title,
          Description = reminderModel.Description,
          Status = reminderModel.Status,
          Priority = reminderModel.Priority,
          RecurringPattern = reminderModel.RecurringPattern,
          ReminderType = reminderModel.ReminderType,
        };
        return reminderDtoResult;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static Reminder ToReminderModel(
      this CreateReminderRequestDto reminderDto,
      string appUserId
    )
    {
      try
      {
        Reminder reminderModelResult = new()
        {
          DueDate = reminderDto.DueDate,
          DueTime = reminderDto.DueTime,
          Title = reminderDto.Title,
          Description = reminderDto.Description,
          Status = reminderDto.Status,
          Priority = reminderDto.Priority,
          RecurringPattern = reminderDto.RecurringPattern,
          ReminderType = reminderDto.ReminderType,
          AppUserId = appUserId,
        };
        return reminderModelResult;
      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
