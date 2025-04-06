using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Contracts.Enums;
using Shared.Dtos.Validators;

namespace Shared.Dtos.Reminder
{
  public class CreateReminderRequestDto
  {
    [DefaultValue("My Reminder Title")]
    [Required(ErrorMessage = "The title is required.")]
    [MinLength(4, ErrorMessage = "The title must be at least 4 characters long.")]
    [MaxLength(50, ErrorMessage = "The title must not excess 50 characters long.")]
    public required string Title { get; set; }

    [DefaultValue("This is a description of the reminder.")]
    [MaxLength(200, ErrorMessage = "The title must not excess 200 characters long.")]
    public required string Description { get; set; }

    [CustomValidation(typeof(DueDateValidator), nameof(DueDateValidator.ValidateFutureDate))]
    [DefaultValue("2025-04-05")]
    public DateOnly DueDate { get; set; }

    [DefaultValue("14:00:00")]
    public TimeOnly DueTime { get; set; }

    // [JsonConverter(typeof(CustomStringToEnumConverter))]
    [Required(ErrorMessage = "The status is required.")]
    [CustomValidation(typeof(EnumValidator), nameof(EnumValidator.ValidateEnum))]
    [DefaultValue("Pending")]
    public required ReminderStatusType Status { get; set; }

    // [JsonConverter(typeof(CustomStringToEnumConverter))]
    [Required(ErrorMessage = "The priority level is required.")]
    [CustomValidation(typeof(EnumValidator), nameof(EnumValidator.ValidateEnum))]
    [DefaultValue("High")]
    public required PriorityLevelType Priority { get; set; }

    // [JsonConverter(typeof(CustomStringToEnumConverter))]
    [Required(ErrorMessage = "The repeat frequency is required.")]
    [CustomValidation(typeof(EnumValidator), nameof(EnumValidator.ValidateEnum))]
    [DefaultValue("None")]
    public required RepeatFrequencyType RepeatFrequency { get; set; }

    // [JsonConverter(typeof(CustomStringToEnumConverter))]
    [Required(ErrorMessage = "The reminder type is required.")]
    [CustomValidation(typeof(EnumValidator), nameof(EnumValidator.ValidateEnum))]
    [DefaultValue("Personal")]
    public required ReminderType ReminderType { get; set; }

    public CreateReminderRequestDto()
    {
      Title = string.Empty;
      Description = string.Empty;
    }
  }
}
