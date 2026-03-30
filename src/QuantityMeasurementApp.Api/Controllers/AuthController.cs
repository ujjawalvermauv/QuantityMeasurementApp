using Microsoft.AspNetCore.Mvc;
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

    public AuthController(
        IUserAuthRepository userRepository,
        IPasswordHashingService passwordHashing,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHashing = passwordHashing;
        _jwtTokenService = jwtTokenService;
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
        return Ok(response);
    }
}
