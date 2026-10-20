using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Cognia.API.Data;
using Cognia.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Cognia.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly CogniaMongoDatabase _database;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<UserAccount> _passwordHasher;

    public AuthController(
        CogniaMongoDatabase database,
        IConfiguration configuration,
        IPasswordHasher<UserAccount> passwordHasher)
    {
        _database = database;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(AccountRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        if (!email.Contains('@') || string.IsNullOrWhiteSpace(request.DisplayName) || request.Password.Length < 12)
        {
            return BadRequest("Provide a valid email, display name, and password of at least 12 characters.");
        }

        var normalizedEmail = email.ToUpperInvariant();
        if (await _database.Accounts.Find(account => account.NormalizedEmail == normalizedEmail)
                .AnyAsync(cancellationToken))
        {
            return Conflict("An account with that email already exists.");
        }

        var account = new UserAccount
        {
            Id = await _database.NextIdAsync("accounts", cancellationToken),
            Email = email,
            NormalizedEmail = normalizedEmail,
            DisplayName = request.DisplayName.Trim(),
            Role = "User"
        };
        account.PasswordHash = _passwordHasher.HashPassword(account, request.Password);
        await _database.Accounts.InsertOneAsync(account, cancellationToken: cancellationToken);

        return Ok(CreateResponse(account));
    }

    [HttpPost("bootstrap-admin")]
    public async Task<ActionResult<AuthResponse>> BootstrapAdmin(AccountRequest request, CancellationToken cancellationToken)
    {
        var configuredToken = _configuration["Admin:BootstrapToken"];
        var suppliedToken = Request.Headers["X-Admin-Bootstrap-Token"].ToString();
        if (string.IsNullOrWhiteSpace(configuredToken) ||
            string.IsNullOrEmpty(suppliedToken) ||
            Encoding.UTF8.GetByteCount(configuredToken) != Encoding.UTF8.GetByteCount(suppliedToken) ||
            !CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(configuredToken), Encoding.UTF8.GetBytes(suppliedToken)))
        {
            return Unauthorized();
        }

        if (await _database.Accounts.Find(account => account.Role == "Admin").AnyAsync(cancellationToken))
        {
            return Conflict("An administrator is already configured.");
        }

        var email = request.Email.Trim();
        if (!email.Contains('@') || string.IsNullOrWhiteSpace(request.DisplayName) || request.Password.Length < 12)
        {
            return BadRequest("Provide a valid email, display name, and password of at least 12 characters.");
        }

        var normalizedEmail = email.ToUpperInvariant();
        if (await _database.Accounts.Find(account => account.NormalizedEmail == normalizedEmail)
                .AnyAsync(cancellationToken))
        {
            return Conflict("An account with that email already exists.");
        }

        var account = new UserAccount
        {
            Id = await _database.NextIdAsync("accounts", cancellationToken),
            Email = email,
            NormalizedEmail = normalizedEmail,
            DisplayName = request.DisplayName.Trim(),
            Role = "Admin"
        };
        account.PasswordHash = _passwordHasher.HashPassword(account, request.Password);
        await _database.Accounts.InsertOneAsync(account, cancellationToken: cancellationToken);
        return Ok(CreateResponse(account));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var account = await _database.Accounts.Find(item => item.NormalizedEmail == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
        if (account is null || _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Email or password is incorrect.");
        }

        return Ok(CreateResponse(account));
    }

    private AuthResponse CreateResponse(UserAccount account)
    {
        var key = _configuration["Jwt:SigningKey"]
            ?? throw new InvalidOperationException("Jwt:SigningKey must be configured.");
        var issuer = _configuration["Jwt:Issuer"] ?? "Cognia";
        var audience = _configuration["Jwt:Audience"] ?? "CogniaClient";
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim(ClaimTypes.Name, account.DisplayName),
            new Claim(ClaimTypes.Role, account.Role)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new AuthResponse(account.Id, account.Email, account.DisplayName, new JwtSecurityTokenHandler().WriteToken(token));
    }
}