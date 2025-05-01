using System;
using System.Text.Json;
using System.Text.Json.Serialization; // Needed for JsonPropertyName
using Microsoft.FeatureManagement;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Aggregation;
using NRedisStack.Search.Literals.Enums;
using Shared.Contracts.Interfaces;
using Shared.Models;
using StackExchange.Redis;

namespace reminderApi.Data;

public class RedisContext : IRedisContext
{
  private readonly ILogger<RedisContext> _systemLogger;
  private readonly IDatabase? _database;
  private readonly ConnectionMultiplexer? _redis;
  private readonly IVariantFeatureManager _featureManager;

  // private readonly RedisStackCommands _redisStackCommands;

  public RedisContext(IVariantFeatureManager featureManager, ILogger<RedisContext> logger)
  {
    // var options = ConfigurationOptions.Parse(connectionString);
    // options.AbortOnConnectFail = false;
    // _redis = ConnectionMultiplexer.Connect(options);
    // _redisStackCommands = new RedisStackCommands(_redis.GetDatabase());
    _systemLogger = logger;
    _featureManager = featureManager;
    try
    {
      _redis = ConnectionMultiplexer.Connect("localhost");
      _database = _redis.GetDatabase();
    }
    catch (Exception ex)
    {
      logger.LogError("Unable to Initiate Redis DB: {Exception}", ex.Message);
    }
  }

  public void StoreReminders(List<Reminder> reminders, string userId)
  {
    try
    {
      foreach (var reminder in reminders)
      {
        string reminderJson = JsonSerializer.Serialize(reminder);
        _database.JSON().Set($"reminder:{userId}:{reminder.Id}", "$", reminderJson);
        _database.JSON().Set($"reminder:{userId}:{reminder.Id}", "$.userId", $"\"{userId}\"");
      }
      Console.WriteLine($"Stored {reminders.Count} reminders for user {userId}");
      return;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error: {ex.Message}");
    }
  }

  public void DeleteReminders(List<int> reminderIdList, string userId)
  {
    foreach (var reminderId in reminderIdList)
    {
      _database.KeyDelete($"reminder:{userId}:{reminderId}");
    }
    Console.WriteLine($"Deleted {reminderIdList.Count} reminders for user {userId}");
  }

  public List<Reminder> GetAllReminders(string userId)
  {
    string escapedUserId = userId.Replace("-", "\\-");
    var query = new Query($"@userId:{{{escapedUserId}}}");
    try
    {
      SearchResult searchResult = _database.FT().Search("idx:reminders", query);
      List<Reminder> reminders = [];
      foreach (Document doc in searchResult.Documents)
      {
        RedisValue jsonPayload = doc["json"];
        if (!jsonPayload.IsNullOrEmpty)
        {
          try
          {
            Reminder? reminderFromResult = JsonSerializer.Deserialize<Reminder>(
              jsonPayload.ToString()
            );
            if (reminderFromResult != null)
            {
              reminders.Add(reminderFromResult);
            }
          }
          catch (JsonException jsonEx)
          {
            Console.WriteLine($"  - Error deserializing document {doc.Id}: {jsonEx.Message}");
            Console.WriteLine($"    Raw JSON payload: {jsonPayload}");
          }
          catch (Exception ex)
          {
            Console.WriteLine(
              $"  - An unexpected error occurred during deserialization for doc {doc.Id}: {ex.Message}"
            );
          }
        }
        else
        {
          Console.WriteLine($"  - Document {doc.Id} has no 'json' payload.");
        }
      }
      return reminders;
    }
    catch (Exception e)
    {
      Console.WriteLine($"Error: {e.Message}");
      return [];
    }
  }
}
