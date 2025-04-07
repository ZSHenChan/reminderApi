namespace Shared.Dtos.Account
{
  public class NewUserDto
  {
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
  }
}
