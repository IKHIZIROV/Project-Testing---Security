using AdventureAPI.Models;

namespace AdventureAPI.Services;

public class KeyshareService
{
    // Dictionary to store keyshares for different rooms
    // In a real application, this would be stored in a database
    private readonly Dictionary<string, string> _keyshares = new()
    {
        { "secret", "a1b2c3d4e5f6g7h8" },
        { "treasure", "x9y8z7w6v5u4t3s2" },
        { "deeper", "mystical_key_share_123" },
        { "win", "victory_keyshare_456" }
    };

    public KeyshareResponse? GetKeyshare(string roomId, string userRole)
    {
        // Only Admin role can access keyshares
        if (userRole != "Admin")
        {
            return new KeyshareResponse
            {
                Message = "Access denied. Admin role required."
            };
        }

        string roomKey = roomId.ToLower();
        if (!_keyshares.ContainsKey(roomKey))
        {
            return new KeyshareResponse
            {
                Message = $"Keyshare not found for room: {roomId}"
            };
        }

        return new KeyshareResponse
        {
            Keyshare = _keyshares[roomKey],
            Message = "Keyshare retrieved successfully."
        };
    }
}

