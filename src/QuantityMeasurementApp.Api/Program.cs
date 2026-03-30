using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Api.Messaging;
using QuantityMeasurementApp.Api.Middleware;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddXmlSerializerFormatters();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        return new BadRequestObjectResult(new
        {
            timestamp = DateTime.UtcNow,
            status = StatusCodes.Status400BadRequest,
            error = "Validation Error",
            message = errors.Length == 0 ? "Request validation failed." : string.Join(" | ", errors),
            path = context.HttpContext.Request.Path.Value
        });
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.SectionName)
);
builder.Services.AddSingleton<IOperationEventPublisher, RabbitMqOperationEventPublisher>();

builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
builder.Services.AddSingleton<IQuantityMeasurementRepository>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("RepositoryBootstrap");

    var connectionString =
        builder.Configuration.GetConnectionString("QuantityMeasurementDb")
        ?? builder.Configuration["Database:ConnectionString"];

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        logger.LogWarning("No SQL connection string provided. Falling back to in-memory cache repository.");
        return QuantityMeasurementCacheRepository.Instance;
    }

    try
    {
        return new QuantityMeasurementDatabaseRepository(connectionString);
    }
    catch (Exception ex)
    {
        // API remains functional even when SQL Server is down.
        logger.LogWarning(ex, "SQL repository unavailable. Falling back to cache repository.");
        return QuantityMeasurementCacheRepository.Instance;
    }
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
