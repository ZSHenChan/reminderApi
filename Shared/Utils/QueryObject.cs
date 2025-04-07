using System.ComponentModel.DataAnnotations;
using Shared.Contracts.Enums;
using Shared.Models;

namespace Shared.Utils
{
  public class QueryObject
  {
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ReminderType? RemiderType { get; set; }
    public ReminderStatusType? ReminderStatus { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; set; } = 20;
  }
}
