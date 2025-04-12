using Shared.Contracts.Enums;
using Shared.Dtos.Reminder;
using Shared.Models;

namespace reminderApi.Mappers
{
  public static class ReminderMapper
  {
    public static ReminderDto ToReminderDto(this Reminder reminderModel)
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

    public static Reminder ToReminderModel(
      this CreateReminderRequestDto reminderDto,
      string appUserId
    )
    {
      Reminder reminderModelResult = new()
      {
        DueDate = reminderDto.DueDate ?? null,
        DueTime = reminderDto.DueTime ?? null,
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
  }
}
