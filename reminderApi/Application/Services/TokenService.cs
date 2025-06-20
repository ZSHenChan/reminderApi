using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Contracts.Interfaces;
using Shared.Models;

namespace reminderApi.Application.Services
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
      _config = configuration;
      _key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(
          _config["Jwt:SigninKey"] ?? throw new ArgumentNullException("Key not found")
        )
      );
    }

    public string CreateToken(AppUser user)
    {
      var claims = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.NameId, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
      };

      var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

      // int num_valid_days =
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(365),
        SigningCredentials = cred,
        Issuer = _config["Jwt:Issuer"],
        Audience = _config["Jwt:Audience"],
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}
