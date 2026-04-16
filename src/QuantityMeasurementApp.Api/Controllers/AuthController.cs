using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuantityMeasurementApp.Api.Contracts;
using QuantityMeasurementApp.Api.Security;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserAuthRepository _userRepository;
    private readonly IPasswordHashingService _passwordHashing;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly GoogleAuthOptions _googleAuthOptions;

    public AuthController(
        IUserAuthRepository userRepository,
        IPasswordHashingService passwordHashing,
        IJwtTokenService jwtTokenService,
        IOptions<GoogleAuthOptions> googleAuthOptions)
    {
        _userRepository = userRepository;
        _passwordHashing = passwordHashing;
        _jwtTokenService = jwtTokenService;
        _googleAuthOptions = googleAuthOptions.Value;
    }

    [HttpPost("signup")]
    public ActionResult<AuthResponseDto> Signup([FromBody] SignupRequestDto request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (_userRepository.GetByEmail(email) != null)
        {
            throw new ArgumentException("A user with this email already exists.");
        }

        // Passwords are never stored as plain text; only PBKDF2 hashes are persisted.
        var passwordHash = _passwordHashing.HashPassword(request.Password);
        var user = new UserAccountEntity(request.FullName.Trim(), email, passwordHash);

        _userRepository.Create(user);

        var response = _jwtTokenService.GenerateToken(user);
        response.Message = $"Account created successfully. Welcome, {user.FullName}.";
        return Ok(response);
    }

    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login([FromBody] LoginRequestDto request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = _userRepository.GetByEmail(email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var isValid = _passwordHashing.VerifyPassword(request.Password, user.PasswordHash);
        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var response = _jwtTokenService.GenerateToken(user);
        response.Message = $"Login successful. Welcome back, {user.FullName}.";
        return Ok(response);
    }

    [HttpPost("google")]
    public async Task<ActionResult<AuthResponseDto>> GoogleLogin([FromBody] GoogleLoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(_googleAuthOptions.ClientId) ||
            _googleAuthOptions.ClientId == "YOUR_GOOGLE_CLIENT_ID")
        {
            throw new InvalidOperationException("Google sign-in is not configured on the API.");
        }

        GoogleJsonWebSignature.Payload payload;
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(
                request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleAuthOptions.ClientId }
                });
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Invalid Google token.");
        }

        var email = payload.Email?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email) || payload.EmailVerified != true)
        {
            throw new UnauthorizedAccessException("Google account email is not verified.");
        }

        var user = _userRepository.GetByEmail(email);
        if (user == null)
        {
            var fullName = string.IsNullOrWhiteSpace(payload.Name)
                ? email.Split('@')[0]
                : payload.Name.Trim();

            // Google users authenticate via federated login, so a random local hash is stored.
            var generatedHash = _passwordHashing.HashPassword(Guid.NewGuid().ToString("N"));
            user = new UserAccountEntity(fullName, email, generatedHash);
            _userRepository.Create(user);
        }

        var response = _jwtTokenService.GenerateToken(user);
        response.Message = $"Google sign-in successful. Welcome, {user.FullName}.";
        return Ok(response);
    }

    [HttpGet("google")]
    public IActionResult GoogleLoginAvailability()
    {
        if (string.IsNullOrWhiteSpace(_googleAuthOptions.ClientId) ||
            _googleAuthOptions.ClientId == "YOUR_GOOGLE_CLIENT_ID")
        {
            return NotFound(new
            {
                message = "Google sign-in is not configured on the API."
            });
        }

        return NoContent();
    }
}
