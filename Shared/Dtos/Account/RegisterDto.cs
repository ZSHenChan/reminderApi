using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Account
{
  public class RegisterDto
  {
    [Required(ErrorMessage = "Email is required.")]
    [DefaultValue("reminderMaster@reminder.com")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "UserName is required.")]
    [DefaultValue("reminderMaster")]
    [MinLength(6, ErrorMessage = "UserName must be at least {2} characters long.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DefaultValue("Password123!")]
    [DataType(DataType.Password)]
    [StringLength(
      100,
      ErrorMessage = "Password must be at least {2} characters long.",
      MinimumLength = 10
    )]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DefaultValue("Password123!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
  }
}
