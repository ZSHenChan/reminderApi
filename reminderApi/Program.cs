using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using personal_ai.Contracts.Interfaces;
using personal_ai.Data;
using personal_ai.Middleware;
using personal_ai.Repository;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateBootstrapLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);

  // Add services to the container.

  builder.Services.Configure<ApiBehaviorOptions>(options =>
  {
    options.SuppressModelStateInvalidFilter = true;
  });

  builder
    .Services.AddControllers(options =>
    {
      options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
      options.Filters.Add<ModelStateActionFilter>();
    })
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
  ;

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(options =>
  {
    options.SwaggerDoc(
      "v1",
      new OpenApiInfo
      {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
          Name = "Example Contact",
          Url = new Uri("https://example.com/contact"),
        },
        License = new OpenApiLicense
        {
          Name = "Example License",
          Url = new Uri("https://example.com/license"),
        },
      }
    );
    options.UseInlineDefinitionsForEnums();
  });
  Log.Information($"Swagger available at: http://localhost:5241/swagger/index.html");

  builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
  );
  builder.Services.AddScoped<IReminderRepository, ReminderRepository>();

  builder.Services.AddSerilog(
    (services, lc) =>
      lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
  );

  WebApplication app = builder.Build();

  // Global exception handling
  app.UseMiddleware<ExceptionHandlingMiddleware>();
  // Specific exception handling for JSON serialization errors
  app.UseMiddleware<JsonExceptionHandlingMiddleware>();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  app.UseHttpsRedirection();

  app.UseAuthorization();

  app.MapControllers();

  try
  {
    await app.RunAsync();
    Log.Information("Application stopped cleanly.");
  }
  catch (HostAbortedException ex)
  {
    Log.Warning(ex, "Application host was aborted.");
  }
}
catch (Exception ex)
  when (ex is not HostAbortedException && ex.Source != "Microsoft.EntityFrameworkCore.Design")
{
  Log.Fatal(ex, "Application start-up failed: " + ex.Message);
}
finally
{
  Log.CloseAndFlush();
}
