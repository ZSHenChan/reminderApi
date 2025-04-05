using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

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
          // Get clean property name
          string propertyName = GetCleanPropertyName(kvp.Key);

          // For enum validation errors, provide a cleaner message
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
          else if (kvp.Key.EndsWith("Dto")) { }
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
    // No action needed
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

  private string ProcessExceptionMessage(Exception exception, string propertyName)
  {
    if (exception == null)
      return "Invalid value provided.";

    // Handle JSON conversion errors for enums
    if (
      exception.Message.Contains("could not be converted to")
      && exception.Message.Contains("Path: $")
    )
    {
      // Extract the enum type name
      var match = Regex.Match(
        exception.Message,
        @"could not be converted to ([^\.]+\.[^\.]+\.[^\s]+)"
      );
      if (match.Success)
      {
        string fullTypeName = match.Groups[1].Value;
        string typeName = fullTypeName.Split('.').Last();

        // Remove "Type" suffix if present
        if (typeName.EndsWith("Type"))
        {
          typeName = typeName.Substring(0, typeName.Length - 4);
        }

        // Try to get available values for the enum
        try
        {
          var type = Type.GetType(fullTypeName);
          if (type != null && type.IsEnum)
          {
            var values = string.Join(", ", Enum.GetNames(type));
            return $"Invalid value for {propertyName}. Must be one of: {values}";
          }
        }
        catch
        {
          // Fall back to a simpler message if we can't access the enum
        }

        return $"Invalid value for {propertyName}. Please provide a valid {typeName} value.";
      }
    }

    return $"Invalid value for {propertyName}. {exception.Message.Split('.')[0]}.";
  }

  public class ValidationErrorResponse
  {
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
  }
}
