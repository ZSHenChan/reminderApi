using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reminderApi.Mappers;
using Shared.Contracts.Interfaces;
using Shared.Dtos.Reminder;
using Shared.Models;
using Shared.Utils;

namespace reminderApi.Controllers;

/// <summary>
/// Controller for managing reminders.
/// </summary>
[ApiController]
[Route("api/reminder")]
public class ReminderController : ControllerBase
{
  private readonly ILogger<ReminderController> _systemLogger;
  private readonly IReminderRepository _reminderRepository;

  public ReminderController(
    ILogger<ReminderController> logger,
    IReminderRepository reminderRepository
  )
  {
    _systemLogger = logger;
    _reminderRepository = reminderRepository;
  }

  /// <summary>
  /// Get all reminders.
  /// </summary>
  /// <returns></returns>
  [HttpGet("{id:int}", Name = "GetReminder")]
  [Authorize]
  public async Task<IActionResult> Get([FromRoute] int id)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    string UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    if (string.IsNullOrEmpty(UserId))
    {
      return Unauthorized("User not found.");
    }

    Reminder? reminder = await _reminderRepository.GetByIdAsync(id, UserId);
    if (reminder == null)
    {
      return NotFound($"Reminder with ID {id} not found.");
    }

    return Ok(ReminderMapper.ToReminderDto(reminder));
  }

  ///<summary>
  /// Get all reminders.
  /// </summary>
  /// <returns></returns>
  [HttpGet("all", Name = "GetAllReminders")]
  [Authorize]
  public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    string UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    if (string.IsNullOrEmpty(UserId))
    {
      return Unauthorized("User not found.");
    }

    List<Reminder> reminders = await _reminderRepository.GetAllAsync(queryObject, UserId);
    var reminderDtoList = reminders.Select(r => ReminderMapper.ToReminderDto(r));
    return Ok(reminderDtoList);
  }

  ///<summary>
  /// Create new reminder.
  /// </summary>
  /// <returns></returns>
  [HttpPost("add", Name = "CreateNewReminder")]
  [Authorize]
  public async Task<IActionResult> PostReminder([FromBody] CreateReminderRequestDto reminderDto)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    string UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    _systemLogger.LogInformation($"User Add Reminder: {UserId}");
    if (string.IsNullOrEmpty(UserId))
    {
      return Unauthorized("User not found.");
    }

    Reminder reminder = ReminderMapper.ToReminderModel(reminderDto, UserId);
    Reminder reminderSql = await _reminderRepository.AddAsync(reminder);
    return CreatedAtRoute(
      "GetReminder",
      new { id = reminderSql.Id },
      ReminderMapper.ToReminderDto(reminderSql)
    );
  }

  [HttpPut("update/{id:int}", Name = "UpdateReminder")]
  [Authorize]
  public async Task<IActionResult> UpdateReminder(
    [FromRoute] int id,
    [FromBody] CreateReminderRequestDto reminderDto
  )
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    Reminder? reminder = await _reminderRepository.UpdateAsync(id, reminderDto);
    if (reminder == null)
    {
      return NotFound($"Reminder with ID {id} not found.");
    }
    return Ok(ReminderMapper.ToReminderDto(reminder));
  }

  [HttpDelete("delete/{id:int}", Name = "DeleteReminder")]
  [Authorize]
  public async Task<IActionResult> DeleteReminder([FromRoute] int id)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    Reminder? reminder = await _reminderRepository.DeleteAsync(id);
    if (reminder == null)
    {
      return NotFound($"Reminder with ID {id} not found.");
    }
    return NoContent();
  }
}
