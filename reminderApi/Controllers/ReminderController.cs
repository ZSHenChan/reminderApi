using Microsoft.AspNetCore.Mvc;
using reminderApi.Data;
using reminderApi.Mappers;
using Shared.Contracts.Interfaces;
using Shared.Dtos.Reminder;
using Shared.Models;
using Shared.Utils;

namespace personal_ai.Controllers;

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
    AppDBContext appDBContext,
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
  public async Task<IActionResult> Get([FromRoute] int id)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    Reminder? reminder = await _reminderRepository.GetByIdAsync(id);
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
  public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    List<Reminder> reminders = await _reminderRepository.GetAllAsync(queryObject);
    var reminderDtoList = reminders.Select(r => ReminderMapper.ToReminderDto(r));
    return Ok(reminderDtoList);
  }

  ///<summary>
  /// Create new reminder.
  /// </summary>
  /// <returns></returns>
  [HttpPost("add", Name = "CreateNewReminder")]
  public async Task<IActionResult> PostReminder([FromBody] CreateReminderRequestDto reminderDto)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    Reminder reminder = ReminderMapper.ToReminderModel(reminderDto);
    Reminder reminderSql = await _reminderRepository.AddAsync(reminder);
    return CreatedAtRoute(
      "GetReminder",
      new { id = reminderSql.Id },
      ReminderMapper.ToReminderDto(reminderSql)
    );
  }

  [HttpPut("update/{id:int}", Name = "UpdateReminder")]
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
