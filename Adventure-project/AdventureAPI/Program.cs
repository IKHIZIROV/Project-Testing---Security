using System.Security.Claims;
using System.Text;
using AdventureAPI.Models;
using AdventureAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Adventure API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyForJWTTokenGenerationThatIsAtLeast32CharactersLong!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AdventureAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AdventureClient";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Register services
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<KeyshareService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Authentication endpoints
app.MapPost("/api/auth/register", (RegisterRequest request, AuthService authService) =>
{
    if (authService.RegisterUser(request, out string errorMessage))
    {
        return Results.Ok(new { Message = "User registered successfully." });
    }
    return Results.BadRequest(new { Message = errorMessage });
})
.WithName("Register")
.WithOpenApi();

app.MapPost("/api/auth/login", (LoginRequest request, AuthService authService) =>
{
    var response = authService.Login(request);
    if (response == null || string.IsNullOrEmpty(response.Token))
    {
        return Results.Unauthorized();
    }
    return Results.Ok(response);
})
.WithName("Login")
.WithOpenApi();

app.MapGet("/api/auth/me", (HttpContext context, AuthService authService) =>
{
    var username = context.User.FindFirst(ClaimTypes.Name)?.Value;
    if (string.IsNullOrEmpty(username))
    {
        return Results.Unauthorized();
    }

    var user = authService.GetUser(username);
    if (user == null)
    {
        return Results.NotFound(new { Message = "User not found." });
    }

    return Results.Ok(new UserInfo
    {
        Username = user.Username,
        Role = user.Role
    });
})
.WithName("GetCurrentUser")
.RequireAuthorization()
.WithOpenApi();

// Keyshare endpoint
app.MapGet("/api/keys/keyshare/{roomId}", (string roomId, HttpContext context, KeyshareService keyshareService) =>
{
    var role = context.User.FindFirst(ClaimTypes.Role)?.Value ?? "Player";
    var response = keyshareService.GetKeyshare(roomId, role);
    
    if (response == null || string.IsNullOrEmpty(response.Keyshare))
    {
        return Results.BadRequest(new { Message = response?.Message ?? "Failed to retrieve keyshare." });
    }
    
    return Results.Ok(response);
})
.WithName("GetKeyshare")
.RequireAuthorization()
.WithOpenApi();

app.Run();
