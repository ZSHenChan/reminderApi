using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace personal_ai.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _systemLogger;

    public ExceptionHandlingMiddleware(
      RequestDelegate next,
      ILogger<ExceptionHandlingMiddleware> logger
    )
    {
      _next = next;
      _systemLogger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentException)
      {
        _systemLogger.LogError("An error occurred: {Exception}", ex.Message);
        await WriteErrorResponseAsync(
          context,
          StatusCodes.Status400BadRequest,
          "Invalid data provided."
        );
      }
      catch (SqlException ex)
      {
        _systemLogger.LogError("SQL error occurred: {Exception}", ex.Message);
        await WriteErrorResponseAsync(
          context,
          StatusCodes.Status500InternalServerError,
          "Database offline. Please try again later."
        );
      }
      catch (DbUpdateException ex)
      {
        _systemLogger.LogError("Error occurred while updating database: {Exception}", ex.Message);
        await WriteErrorResponseAsync(
          context,
          StatusCodes.Status500InternalServerError,
          "Unable to update Database. Please try again later."
        );
      }
      catch (Exception ex)
      {
        _systemLogger.LogError(
          "An unexpected error occurred. Type: {Type}, Message: {Message}",
          ex.GetType().Name,
          ex.Message
        );
        await WriteErrorResponseAsync(
          context,
          StatusCodes.Status500InternalServerError,
          "An unexpected error occurred. Please try again later."
        );
      }
    }

    private async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = statusCode;

      var problemDetails = new ProblemDetails
      {
        Status = statusCode,
        Title = message,
        Detail = "Please contact support if the issue persists.",
        Instance = context.Request.Path,
      };

      await context.Response.WriteAsJsonAsync(problemDetails);
    }
  }
}
