using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace reminderApi.Infrastructure.Middleware
{
  public class JsonExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public JsonExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        // Read the raw request body
        httpContext.Request.EnableBuffering();
        using var reader = new StreamReader(httpContext.Request.Body);
        var requestBody = await reader.ReadToEndAsync();
        httpContext.Request.Body.Position = 0; // Reset the stream position

        // Attempt to deserialize the JSON
        if (!string.IsNullOrEmpty(requestBody))
        {
          try
          {
            //This is just to test if the json is valid. You do not need to store the result.
            JsonDocument.Parse(requestBody);
          }
          catch (JsonException jsonException)
          {
            await HandleJsonExceptionAsync(httpContext, jsonException);
            return; // Stop further processing
          }
        }

        await _next(httpContext);
      }
      catch (JsonException jsonException)
      {
        await HandleJsonExceptionAsync(httpContext, jsonException);
      }
      catch (Exception ex) when (IsJsonException(ex))
      {
        await HandleJsonExceptionAsync(httpContext, ex);
      }
    }

    private static bool IsJsonException(Exception ex)
    {
      // Check if this is a JSON deserialization exception

      return ex.Message.Contains("Error converting value")
        || ex.Message.Contains("JSON value could not be converted");
    }

    private static async Task HandleJsonExceptionAsync(HttpContext context, Exception exception)
    {
      context.Response.ContentType = "application/json";

      context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

      var errorResponse = new
      {
        title = "Validation Failed",

        status = 400,

        message = "JsonExceptionHandlingMiddleware: Please check the provided data and correct the errors.",

        errors = GetCustomErrorMessages(exception),
      };

      await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private static object GetCustomErrorMessages(Exception exception)
    {
      var errors = new Dictionary<string, string[]>();

      // Extract the property name and enum type from the exception message

      var propertyMatch = Regex.Match(exception.Message, @"Path '([^']+)'|Path: \$\.([^,|]+)");

      if (propertyMatch.Success)
      {
        string propertyName = propertyMatch.Groups[1].Value;

        if (string.IsNullOrEmpty(propertyName))
          propertyName = propertyMatch.Groups[2].Value;

        // Extract the invalid value

        var valueMatch = Regex.Match(exception.Message, @"value\s+""([^""]+)""");

        string invalidValue = valueMatch.Success ? valueMatch.Groups[1].Value : "invalid value";

        // Extract the enum type name

        var typeMatch = Regex.Match(exception.Message, @"type '([^']+)'");

        if (typeMatch.Success)
        {
          string fullTypeName = typeMatch.Groups[1].Value;

          string typeName = fullTypeName.Split('.').Last();

          // Remove "Type" suffix if present

          if (typeName.EndsWith("Type"))
          {
            typeName = typeName.Substring(0, typeName.Length - 4);
          }

          errors[propertyName] = new[]
          {
            $"Invalid value '{invalidValue}' for {propertyName}. Please provide a valid {typeName}.",
          };
        }
        else
        {
          errors[propertyName] = new[] { $"Invalid value for {propertyName}." };
        }
      }
      else if (exception.Message.Contains("required"))
      {
        var requiredMatch = Regex.Match(exception.Message, @"'([^']+)' is required");

        if (requiredMatch.Success)
        {
          string fieldName = requiredMatch.Groups[1].Value;

          errors[fieldName] = new[] { $"The {fieldName} field is required." };
        }
        else
        {
          errors["request"] = new[] { "Request body is required." };
        }
      }
      else
      {
        errors["request"] = new[] { "Invalid request format. Please check your JSON syntax." };
      }

      return errors;
    }
  }
}
