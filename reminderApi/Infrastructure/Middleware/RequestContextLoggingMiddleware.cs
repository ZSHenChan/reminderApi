using System.Net;
using Serilog.Context;
using UAParser;
using UAParser.Objects;

namespace reminderApi.Infrastructure.Middleware;

public class RequestContextLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private static readonly Parser _uaParser = Parser.GetDefault();

  // private readonly IGeoIpService _geoIpService;

  public RequestContextLoggingMiddleware(RequestDelegate next)
  {
    _next = next;
    // _geoIpService = geoIpService;
  }

  public Task InvokeAsync(HttpContext context)
  {
    string userAgent = context.Request.Headers["User-Agent"].ToString();
    string os = "unknown";
    string browser = "unknown";
    string device = "unknown";

    if (!string.IsNullOrEmpty(userAgent) && _uaParser != null) // Check if parser initialized
    {
      try
      {
        // Parse the User-Agent string
        ClientInfo c = _uaParser.Parse(userAgent);
        os = $"{c.OS.Family} {c.OS.Major}.{c.OS.Minor}";
        browser = $"{c.Browser.Family} {c.Browser.Major}.{c.Browser.Minor}";
        device = $"{c.Device.Family}"; // Often 'Other' for desktops
      }
      catch (Exception)
      {
        os = "parsing_error";
      }
    }

    var ipAddressString = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    IPAddress.TryParse(ipAddressString, out var ipAddressObject);

    // string? city = null;
    // string? country = null;
    // if (ipAddressObject != null)
    // {
    //   (city, country) = _geoIpService.TryGetLocation(ipAddressObject);
    // }

    using (LogContext.PushProperty("ClientIP", ipAddressString))
    using (LogContext.PushProperty("Device/OS/Browser", device + " - " + os + " - " + browser))
    {
      return _next(context);
    }
    // Properties are automatically removed when the 'using' block exits
  }
}
