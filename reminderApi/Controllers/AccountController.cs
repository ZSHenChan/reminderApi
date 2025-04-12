using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Interfaces;
using Shared.Dtos.Account;
using Shared.Models;
using Shared.Reponses;

namespace reminderApi.Controllers
{
  [Route("api/account")]
  public class AccountController : Controller
  {
    private readonly ILogger<AccountController> _systemLogger;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(
      ILogger<AccountController> logger,
      UserManager<AppUser> userManager,
      ITokenService tokenService,
      SignInManager<AppUser> signInManager
    )
    {
      _systemLogger = logger;
      _userManager = userManager;
      _tokenService = tokenService;
      _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var user = await _userManager.FindByEmailAsync(loginDto.Email);
      if (user == null)
        return Unauthorized("User not found");

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
      if (!result.Succeeded)
        return Unauthorized("Email not found and/or password is incorrect");

      return Ok(
        new NewUserDto
        {
          Email = user.Email,
          UserName = user.UserName,
          Token = _tokenService.CreateToken(user),
        }
      );
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
        return Ok(
          new NewUserDto
          {
            Email = user.Email,
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
          }
        );
      }

      foreach (var error in result.Errors)
      {
        ModelState.AddModelError(string.Empty, error.Description);
      }

      return BadRequest(ModelState);
    }
  }
}
