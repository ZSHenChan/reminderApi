using personal_ai.Contracts.Enums;
using personal_ai.Dtos.Reminder;
using personal_ai.Models;
using personal_ai.Utils;

namespace personal_ai.Mappers
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
                    IsRecurring = reminderModel.IsRecurring,
                    Status = reminderModel.Status,
                    Priority = reminderModel.Priority,
                    RepeatFrequency = reminderModel.RepeatFrequency,
                    ReminderType = reminderModel.ReminderType,
                };
                return reminderDtoResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Reminder ToReminderModel(this CreateReminderRequestDto reminderDto)
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
                    RepeatFrequency = reminderDto.RepeatFrequency,
                    ReminderType = reminderDto.ReminderType,
                    IsRecurring = reminderDto.RepeatFrequency != RepeatFrequencyType.None,
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
