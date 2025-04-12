namespace Shared.Contracts.Interfaces;

public interface IValidationErrorResponse
{
  string Title { get; set; }
  int Status { get; set; }
  string Message { get; set; }
  Dictionary<string, string[]> Errors { get; set; }
}
