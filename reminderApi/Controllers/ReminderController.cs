using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using reminderApi.Data;
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
  private readonly IRedisContext _redisContext;
  private readonly IVariantFeatureManager _featureManager;

  public ReminderController(
    ILogger<ReminderController> logger,
    IReminderRepository reminderRepository,
    IRedisContext redisContext,
    IVariantFeatureManager featureManager
  )
  {
    _systemLogger = logger;
    _reminderRepository = reminderRepository;
    _redisContext = redisContext;
    _featureManager = featureManager;
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

    List<Reminder> reminders = [];
    if (await _featureManager.IsEnabledAsync("FeatureRedis"))
    {
      reminders = _redisContext.GetAllReminders(UserId);
      if (reminders.Count != 0)
      {
        return Ok(reminders.Select(r => ReminderMapper.ToReminderDto(r)));
      }
      Console.WriteLine("No reminders found in Redis, fetching from SQL Server.");
    }

    reminders = await _reminderRepository.GetAllAsync(queryObject, UserId);
    if (await _featureManager.IsEnabledAsync("FeatureRedis"))
      _redisContext.StoreReminders(reminders, UserId);
    var reminderDtoList = reminders.Select(r => ReminderMapper.ToReminderDto(r));
    return Ok(reminderDtoList);
  }

  ///<summary>
  /// Create new reminder.
  /// </summary>
  /// <returns></returns>
  [HttpPost("add", Name = "CreateNewReminder")]
  [Authorize]
  public async Task<IActionResult> CreateReminder(
    [FromBody] CreateReminderRequestDto[] reminderDtoList
  )
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    string UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    if (string.IsNullOrEmpty(UserId))
    {
      return Unauthorized("User not found.");
    }

    Reminder[] reminderList =
    [
      .. reminderDtoList.Select(reminderDto => ReminderMapper.ToReminderModel(reminderDto, UserId)),
    ];
    if (await _featureManager.IsEnabledAsync("FeatureRedis"))
      _redisContext.StoreReminders([.. reminderList], UserId);
    Reminder[] failedReminders = await _reminderRepository.AddAsync(reminderList);
    return Ok(failedReminders);
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

    string UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    if (string.IsNullOrEmpty(UserId))
    {
      return Unauthorized("User not found.");
    }

    if (await _featureManager.IsEnabledAsync("FeatureRedis"))
      _redisContext.DeleteReminders([id], UserId);
    Reminder? reminder = await _reminderRepository.DeleteAsync(id, UserId);
    if (reminder == null)
    {
      return NotFound($"Reminder with ID {id} not found.");
    }
    return NoContent();
  }
}
