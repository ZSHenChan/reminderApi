using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Dtos.Account;
using Shared.Models;

namespace personal_ai.Controllers
{
  [Route("api/account")]
  public class AccountController : Controller
  {
    private readonly ILogger<AccountController> _systemLogger;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager)
    {
      _systemLogger = logger;
      _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var user = new AppUser { UserName = registerDto.UserName, Email = registerDto.Email };

      var result = await _userManager.CreateAsync(user, registerDto.Password);
      if (result.Succeeded)
      {
        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
          foreach (var error in roleResult.Errors)
          {
            ModelState.AddModelError(string.Empty, error.Description);
          }
          return BadRequest(ModelState);
        }
        return Ok("User registered successfully.");
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return BadRequest(ModelState);
    }
  }
}
