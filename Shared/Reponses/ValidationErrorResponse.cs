using Shared.Contracts.Interfaces;

namespace Shared.Reponses;

public class ValidationErrorResponse : IValidationErrorResponse
{
  public string Title { get; set; } = string.Empty;
  public int Status { get; set; }
  public string Message { get; set; } = string.Empty;
  public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}
