using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Account
{
  public class LoginDto
  {
    [Required(ErrorMessage = "{0} is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
  }
}
