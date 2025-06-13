using System;
using System.IO;
using System.Net;
using MaxMind.GeoIP2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace reminderApi.Application.Services;

public interface IGeoIpService
{
  (string? City, string? Country) TryGetLocation(IPAddress ipAddress);
}

public class GeoIpService : IGeoIpService, IDisposable
{
  private readonly DatabaseReader? _reader;
  private readonly ILogger<GeoIpService> _logger;

  public GeoIpService(IConfiguration configuration, ILogger<GeoIpService> logger)
  {
    _logger = logger;
    var dbPath = configuration["GeoIp:DatabasePath"]; // Get path from config

    if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath))
    {
      _logger.LogWarning(
        "GeoIP Database path not configured or file not found: {DbPath}. Location logging disabled.",
        dbPath
      );
      _reader = null;
      return;
    }

    try
    {
      _logger.LogInformation("Initializing GeoIP Database reader from: {DbPath}", dbPath);
      _reader = new DatabaseReader(dbPath);
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "Failed to initialize GeoIP Database reader from: {DbPath}. Location logging disabled.",
        dbPath
      );
      _reader = null;
    }
  }

  public (string? City, string? Country) TryGetLocation(IPAddress ipAddress)
  {
    if (_reader == null || ipAddress == null || IPAddress.IsLoopback(ipAddress))
    {
      return (null, null); // Don't lookup loopback or if reader failed
    }

    try
    {
      if (_reader.TryCity(ipAddress, out var response) && response != null)
      {
        return (response.City?.Name, response.Country?.IsoCode); // e.g., "Singapore", "SG"
      }
    }
    catch (Exception ex)
    {
      // Log lookup errors - might happen for specific IPs or db issues
      _logger.LogWarning(ex, "GeoIP lookup failed for IP: {IPAddress}", ipAddress);
    }
    return (null, null);
  }

  public void Dispose()
  {
    _reader?.Dispose();
    GC.SuppressFinalize(this);
  }
}
