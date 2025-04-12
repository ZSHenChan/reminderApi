using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Account
{
  public class LoginDto
  {
    [Required(ErrorMessage = "{0} is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} is required.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
  }
}
