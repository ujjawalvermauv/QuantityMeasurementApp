using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantityMeasurementApp.Api.Middleware;
using QuantityMeasurementApp.Api.Security;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Repository;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000" , "https://quantity-measurement-app-frontend.onrender.com" };
const string corsPolicyName = "AllowedOriginsPolicy";

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
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    // Swagger bearer definition allows login token testing directly from UI.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token as: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "https://quantity-measurement-app-frontend.onrender.com",
                "http://127.0.0.1:5500",
                "http://localhost:5500",
                "http://192.168.1.34:5500",
                "http://127.0.0.1:3000",
                "http://localhost:3000",
                "http://127.0.0.1:3001",
                "http://localhost:3001",
                "http://127.0.0.1:5173",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName)
);
builder.Services.Configure<GoogleAuthOptions>(
    builder.Configuration.GetSection(GoogleAuthOptions.SectionName)
);

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = signingKey,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IPasswordHashingService, Pbkdf2PasswordHashingService>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

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

builder.Services.AddSingleton<IUserAuthRepository>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("UserRepositoryBootstrap");
    var fallbackRepository = UserAuthCacheRepository.Instance;
    var connectionString =
        builder.Configuration.GetConnectionString("QuantityMeasurementDb")
        ?? builder.Configuration["Database:ConnectionString"];

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        logger.LogWarning("No SQL connection string provided for auth. Falling back to in-memory auth repository.");
        return fallbackRepository;
    }

    IUserAuthRepository primaryRepository;
    try
    {
        // Repository creates Users table automatically when missing.
        primaryRepository = new UserAuthDatabaseRepository(connectionString);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Could not initialize SQL user repository. Falling back to in-memory auth repository.");
        return fallbackRepository;
    }

    return new UserAuthResilientRepository(
        primaryRepository,
        fallbackRepository,
        serviceProvider.GetRequiredService<ILogger<UserAuthResilientRepository>>()
    );
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
