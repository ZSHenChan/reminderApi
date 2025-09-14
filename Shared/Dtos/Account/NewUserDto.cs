namespace Shared.Dtos.Account
{
  public class NewUserDto
  {
    public required string Email { get; set; } = string.Empty;
    public required string UserName { get; set; } = string.Empty;
    public required string Token { get; set; } = string.Empty;
  }
}
