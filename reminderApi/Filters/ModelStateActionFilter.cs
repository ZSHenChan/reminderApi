using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Reponses;

namespace reminderApi.Filters;

public class ModelStateActionFilter : IActionFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      var errors = new Dictionary<string, string[]>();

      foreach (var kvp in context.ModelState)
      {
        if (kvp.Value?.Errors.Count > 0)
        {
          string propertyName = GetCleanPropertyName(kvp.Key);

          if (kvp.Value.Errors.Any(e => e.ErrorMessage.Contains("could not be converted to")))
          {
            // Extract the enum type name
            var errorMsg = kvp.Value.Errors.First().ErrorMessage;
            var match = Regex.Match(errorMsg, @"Path: \$\.([^|]+)");

            string fieldName = match.Success ? match.Groups[1].Value.Trim() : propertyName;

            errors[fieldName] = new[]
            {
              $"Invalid value for {{ {fieldName} }}. Please provide a valid value.",
            };
          }
          // else if (kvp.Key.EndsWith("Dto")) { }
          else
          {
            var errorMessages = kvp
              .Value.Errors.Select(e =>
                !string.IsNullOrWhiteSpace(e.ErrorMessage)
                  ? e.ErrorMessage
                  : $"Invalid value for {propertyName}."
              )
              .ToArray();

            errors[propertyName] = errorMessages;
          }
        }
      }

      var response = new ValidationErrorResponse
      {
        Title = "Validation Failed",
        Status = 400,
        Message = "Please check the provided data and correct the errors.",
        Errors = errors,
      };

      context.Result = new BadRequestObjectResult(response);
    }
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    if (!context.ModelState.IsValid)
    {
      var errors = new Dictionary<string, string[]>();

      foreach (var kvp in context.ModelState)
      {
        if (kvp.Value?.Errors.Count > 0)
        {
          string propertyName = GetCleanPropertyName(kvp.Key);

          var errorMessages = kvp
            .Value.Errors.Select(e =>
              !string.IsNullOrWhiteSpace(e.ErrorMessage)
                ? e.ErrorMessage
                : $"Invalid value for {propertyName}."
            )
            .ToArray();

          errors[propertyName] = errorMessages;
        }
      }

      var response = new ValidationErrorResponse
      {
        Title = "Validation Failed",
        Status = 400,
        Message = "Please check the provided data and correct the errors.",
        Errors = errors,
      };

      context.Result = new BadRequestObjectResult(response);
    }
  }

  private string GetCleanPropertyName(string key)
  {
    if (key.StartsWith("$."))
    {
      return key.Substring(2);
    }

    if (key.Contains("."))
    {
      return key.Substring(key.LastIndexOf('.') + 1);
    }

    return key;
  }
}
