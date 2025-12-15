using System.Security.Cryptography;
using System.Text;
using AdventureAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AdventureAPI.Services;

public class AuthService
{
    private readonly Dictionary<string, User> _users = new();
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        string passwordHash = HashPassword(password);
        return passwordHash == hash;
    }

    public string GenerateJwtToken(string username, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "AdventureAPI",
            audience: _configuration["Jwt:Audience"] ?? "AdventureClient",
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool RegisterUser(RegisterRequest request, out string errorMessage)
    {
        errorMessage = string.Empty;

        // Input validation
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            errorMessage = "Username is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errorMessage = "Password is required.";
            return false;
        }

        if (request.Username.Length < 3)
        {
            errorMessage = "Username must be at least 3 characters long.";
            return false;
        }

        if (request.Password.Length < 6)
        {
            errorMessage = "Password must be at least 6 characters long.";
            return false;
        }

        // Check if user already exists
        if (_users.ContainsKey(request.Username.ToLower()))
        {
            errorMessage = "Username already exists.";
            return false;
        }

        // Create new user
        var user = new User
        {
            Username = request.Username,
            PasswordHash = HashPassword(request.Password),
            Role = request.Role ?? "Player"
        };

        _users[request.Username.ToLower()] = user;
        return true;
    }

    public LoginResponse? Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponse
            {
                Message = "Username and password are required."
            };
        }

        string usernameKey = request.Username.ToLower();
        if (!_users.ContainsKey(usernameKey))
        {
            return new LoginResponse
            {
                Message = "Invalid username or password."
            };
        }

        var user = _users[usernameKey];

        // Check if account is locked
        if (user.LockedUntil.HasValue && user.LockedUntil.Value > DateTime.Now)
        {
            var remainingTime = user.LockedUntil.Value - DateTime.Now;
            return new LoginResponse
            {
                Message = $"Account is locked. Try again in {remainingTime.Minutes} minute(s) and {remainingTime.Seconds} second(s)."
            };
        }

        // Verify password
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            
            if (user.FailedLoginAttempts >= 3)
            {
                user.LockedUntil = DateTime.Now.AddMinutes(15);
                user.FailedLoginAttempts = 0;
                return new LoginResponse
                {
                    Message = "Too many failed login attempts. Account locked for 15 minutes."
                };
            }

            return new LoginResponse
            {
                Message = $"Invalid username or password. {3 - user.FailedLoginAttempts} attempt(s) remaining."
            };
        }

        // Successful login - reset failed attempts
        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;

        string token = GenerateJwtToken(user.Username, user.Role);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            Message = "Login successful."
        };
    }

    public User? GetUser(string username)
    {
        string usernameKey = username.ToLower();
        return _users.ContainsKey(usernameKey) ? _users[usernameKey] : null;
    }
}

