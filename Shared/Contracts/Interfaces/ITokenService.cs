using Shared.Models;

namespace Shared.Contracts.Interfaces
{
  public interface ITokenService
  {
    string CreateToken(AppUser user);
  }
}
