using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using personal_ai.Contracts.Interfaces;
using personal_ai.Data;
using personal_ai.Dtos.Reminder;
using personal_ai.Mappers;
using personal_ai.Models;
using personal_ai.Utils;

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
  public async Task<IActionResult> GetAll()
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    List<Reminder> reminders = await _reminderRepository.GetAllAsync();
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
