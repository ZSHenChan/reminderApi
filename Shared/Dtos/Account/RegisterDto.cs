using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Account
{
  public class RegisterDto
  {
    [Required(ErrorMessage = "{0} is required.")]
    [DefaultValue("reminderMaster@reminder.com")]
    [EmailAddress(ErrorMessage = "Invalid {0}.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [DefaultValue("reminderMaster")]
    [StringLength(
      20,
      MinimumLength = 5,
      ErrorMessage = "{0} must be between {2} and {1} characters long."
    )]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [DefaultValue("Password123!")]
    [DataType(DataType.Password)]
    [StringLength(
      100,
      MinimumLength = 10,
      ErrorMessage = "{0} must be at least {2} characters long."
    )]
    public required string Password { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [DefaultValue("Password123!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "{0} does not match.")]
    public required string ConfirmPassword { get; set; }
  }
}
