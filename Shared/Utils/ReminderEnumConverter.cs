namespace Shared.Utils;

using System;
using Shared.Contracts.Enums;
using Shared.Dtos.Reminder;
using Shared.Models;

public static class ReminderEnumConverter
{
  public static TEnum FromIndex<TEnum>(int index)
    where TEnum : Enum
  {
    if (Enum.IsDefined(typeof(TEnum), index))
    {
      return (TEnum)Enum.ToObject(typeof(TEnum), index);
    }
    throw new ArgumentOutOfRangeException(
      nameof(index),
      $"Invalid index for {typeof(TEnum).Name} enum."
    );
  }

  public static int ToIndex<TEnum>(TEnum enumValue)
    where TEnum : Enum
  {
    return Convert.ToInt32(enumValue);
  }

  public static TEnum FromString<TEnum>(string enumString)
    where TEnum : Enum
  {
    if (Enum.TryParse(typeof(TEnum), enumString, out var result))
    {
      return (TEnum)result;
    }
    throw new ArgumentException(
      $"Invalid string for {typeof(TEnum).Name} enum: {enumString}",
      nameof(enumString)
    );
  }

  public static void UpdateReminderSql(Reminder reminderSql, CreateReminderRequestDto reminderDto)
  {
    try
    {
      reminderSql.DueDate = reminderDto.DueDate;
      reminderSql.DueTime = reminderDto.DueTime;
      reminderSql.Title = reminderDto.Title;
      reminderSql.Description = reminderDto.Description;
      reminderSql.Status = reminderDto.Status;
      reminderSql.Priority = reminderDto.Priority;
      reminderSql.RecurringPattern = reminderDto.RecurringPattern;
      reminderSql.ReminderType = reminderDto.ReminderType;
    }
    catch (Exception)
    {
      throw;
    }
  }
}
