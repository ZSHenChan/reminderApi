using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using reminderApi.Data;
using reminderApi.Middleware;
using reminderApi.Repository;
using reminderApi.Service;
using Serilog;
using Serilog.Events;
using Shared.Contracts.Interfaces;
using Shared.Models;

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
    options.AddSecurityDefinition(
      "Bearer",
      new OpenApiSecurityScheme
      {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
      }
    );
    options.AddSecurityRequirement(
      new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
          },
          new string[] { }
        },
      }
    );
  });
  Log.Information($"Swagger available at: http://localhost:5241/swagger/index.html");

  builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
  );

  builder
    .Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
      options.User.RequireUniqueEmail = true;
      options.Password.RequiredLength = 10;
      options.Password.RequireDigit = true;
      options.Password.RequireLowercase = false;
      options.Password.RequireUppercase = true;
      options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDBContext>();

  builder
    .Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
        options.DefaultForbidScheme =
        options.DefaultSignInScheme =
        options.DefaultSignOutScheme =
          JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
          System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigninKey"])
        ),
      };
    });

  builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
  builder.Services.AddScoped<ITokenService, TokenService>();

  builder.Logging.ClearProviders();
  builder.Host.UseSerilog(
    (context, services, configuration) =>
    {
      configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
    }
  );
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

  app.UseAuthentication();
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
