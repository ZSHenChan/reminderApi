using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Contracts.Enums;
using Shared.Dtos.Validators;
using Shared.Models;

namespace Shared.Dtos.Reminder
{
  public class CreateReminderRequestDto
  {
    [DefaultValue("My Reminder Title")]
    [Required(ErrorMessage = "{0} is required.")]
    [MaxLength(50, ErrorMessage = "The title must not excess 50 characters long.")]
    public required string Title { get; set; }

    [DefaultValue("Describe your reminder")]
    [MaxLength(10, ErrorMessage = "{0} must not exceed {1} characters long.")]
    public string Description { get; set; }

    [CustomValidation(typeof(DueDateValidator), nameof(DueDateValidator.ValidateFutureDate))]
    [DefaultValue("2025-04-05")]
    public DateOnly? DueDate { get; set; }

    [DefaultValue("14:00:00")]
    public TimeOnly? DueTime { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [CustomValidation(typeof(EnumValidator), nameof(EnumValidator.ValidateEnum))]
    [DefaultValue("Pending")]
    public required ReminderStatusType Status { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [CustomValidation(typeof(EnumValidator), nameof(EnumValidator.ValidateEnum))]
    [DefaultValue("High")]
    public required PriorityLevelType Priority { get; set; }

    public RecurringPattern? RecurringPattern { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
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
