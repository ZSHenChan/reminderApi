using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Validators
{
  public static class DueDateValidator
  {
    public static ValidationResult? ValidateFutureDate(DateOnly dueDate, ValidationContext context)
    {
      if (dueDate < DateOnly.FromDateTime(DateTime.Now))
      {
        return new ValidationResult("The due date must be today or onwards.");
      }
      return ValidationResult.Success;
    }
  }
}
