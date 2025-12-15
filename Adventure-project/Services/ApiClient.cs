using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Adventure_project.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private string? _jwtToken;
    private string? _username;
    private string? _userRole;

    public string? Username => _username;
    public string? UserRole => _userRole;
    public bool IsAuthenticated => !string.IsNullOrEmpty(_jwtToken);

    public ApiClient(string apiBaseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(apiBaseUrl)
        };
    }

    public void SetToken(string token)
    {
        _jwtToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string? role = null)
    {
        try
        {
            var request = new { Username = username, Password = password, Role = role };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Registration successful!");
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, $"Registration failed: {errorContent}");
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Token, string Username, string Role, string Message)> LoginAsync(string username, string password)
    {
        try
        {
            var request = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
            
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    _username = loginResponse.Username;
                    _userRole = loginResponse.Role;
                    SetToken(loginResponse.Token);
                    return (true, loginResponse.Token, loginResponse.Username, loginResponse.Role, loginResponse.Message);
                }
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, string.Empty, string.Empty, string.Empty, $"Login failed: {errorContent}");
        }
        catch (Exception ex)
        {
            return (false, string.Empty, string.Empty, string.Empty, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool Success, string Keyshare, string Message)> GetKeyshareAsync(string roomId)
    {
        if (!IsAuthenticated)
        {
            return (false, string.Empty, "Not authenticated. Please login first.");
        }

        try
        {
            var response = await _httpClient.GetAsync($"/api/keys/keyshare/{roomId}");
            
            if (response.IsSuccessStatusCode)
            {
                var keyshareResponse = await response.Content.ReadFromJsonAsync<KeyshareResponse>();
                if (keyshareResponse != null)
                {
                    return (true, keyshareResponse.Keyshare, keyshareResponse.Message);
                }
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, string.Empty, $"Failed to get keyshare: {errorContent}");
        }
        catch (Exception ex)
        {
            return (false, string.Empty, $"Error: {ex.Message}");
        }
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    private class KeyshareResponse
    {
        public string Keyshare { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

