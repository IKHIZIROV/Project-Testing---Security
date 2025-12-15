using Adventure_project.Services;

namespace Adventure_project;

// main

public enum Directions
{
    North, East, South, West, Up, Down
}

internal class Program
{
    private static ApiClient? _apiClient;
    private static string? _userRole;

    static async Task Main(string[] args)
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("       WELCOME TO THE CASTLE ADVENTURE!         ");
        Console.WriteLine("=================================================\n");

        // Initialize API client
        string apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5196";
        _apiClient = new ApiClient(apiUrl);

        // Login or register
        if (!await AuthenticateUser())
        {
            Console.WriteLine("Authentication failed. Exiting...");
            return;
        }

        // Initialize the game
        GameSetup setup = new GameSetup();
        Rooms world = setup.CreateWorld();

        // Add monsters to specific rooms
        Monster orc = Monster.CreateOrc();
        world.AddMonsterToRoom("Deeper", orc);

        // Game loop
        bool playing = true;

        while (playing && !world.GameOver)
        {
            Console.Write("\nWhat do you want to do? > ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            // Secure input handling - sanitize input
            input = SanitizeInput(input);
            string lowerInput = input.ToLower();

            string[] parts = lowerInput.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0] : "";
            string argument = parts.Length > 1 ? parts[1] : "";

            try
            {
                switch (command)
                {
                    case "north":
                    case "n":
                        world.Move(Directions.North);
                        await HandleEncryptedRoom(world);
                        break;

                    case "south":
                    case "s":
                        world.Move(Directions.South);
                        await HandleEncryptedRoom(world);
                        break;

                    case "east":
                    case "e":
                        world.Move(Directions.East);
                        await HandleEncryptedRoom(world);
                        break;

                    case "west":
                    case "w":
                        world.Move(Directions.West);
                        await HandleEncryptedRoom(world);
                        break;

                    case "up":
                    case "u":
                        world.Move(Directions.Up);
                        await HandleEncryptedRoom(world);
                        break;

                    case "down":
                    case "d":
                        world.Move(Directions.Down);
                        await HandleEncryptedRoom(world);
                        break;

                    case "look":
                    case "l":
                        world.Look();
                        break;

                    case "inventory":
                    case "inv":
                    case "i":
                        world.PlayerInv.ShowInventory();
                        break;

                    case "take":
                    case "get":
                    case "pickup":
                        if (string.IsNullOrWhiteSpace(argument))
                        {
                            Console.WriteLine("Take what? Specify an item name.");
                        }
                        else
                        {
                            world.TakeItem(argument);
                        }
                        break;

                    case "fight":
                    case "attack":
                    case "combat":
                        world.FightMonster();
                        break;

                    case "status":
                    case "stats":
                        world.ShowStatus();
                        break;

                    case "noclip":
                        if (_userRole == "Admin")
                        {
                            await HandleNoclip(world, argument);
                        }
                        else
                        {
                            Console.WriteLine("Access denied. Admin role required.");
                        }
                        break;

                    case "decrypt":
                        if (!string.IsNullOrWhiteSpace(argument))
                        {
                            await HandleDecryptRoom(world, argument);
                        }
                        else
                        {
                            Console.WriteLine("Usage: decrypt <room_name>");
                        }
                        break;

                    case "help":
                    case "h":
                    case "?":
                        ShowHelp(_userRole == "Admin");
                        break;

                    case "quit":
                    case "exit":
                    case "q":
                        Console.WriteLine("Thanks for playing! Goodbye!");
                        playing = false;
                        break;

                    default:
                        Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Secure error handling - don't expose internal details
                Console.WriteLine("An error occurred. Please try again.");
                // In development, you might want to log the exception
                #if DEBUG
                Console.WriteLine($"Debug: {ex.Message}");
                #endif
            }

            // Check game over conditions
            if (world.GameOver)
            {
                if (world.GameWon)
                {
                    Console.WriteLine("\n🏆 You completed the adventure! Well done, brave hero! 🏆");
                }
                else
                {
                    Console.WriteLine("\n💀 Game Over. Better luck next time! 💀");
                }

                Console.WriteLine("\nWould you like to play again? (yes/no)");
                string? playAgain = Console.ReadLine()?.Trim().ToLower();
                if (playAgain == "yes" || playAgain == "y")
                {
                    // Restart the game
                    world = setup.CreateWorld();
                    Monster newOrc = Monster.CreateOrc();
                    world.AddMonsterToRoom("Deeper", newOrc);
                }
                else
                {
                    playing = false;
                }
            }
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task<bool> AuthenticateUser()
    {
        Console.WriteLine("=== Authentication Required ===");
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.Write("Choose an option (1 or 2): ");

        string? choice = Console.ReadLine()?.Trim();

        if (choice != "1" && choice != "2")
        {
            Console.WriteLine("Invalid choice.");
            return false;
        }

        Console.Write("Username: ");
        string? username = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return false;
        }

        Console.Write("Password: ");
        string? password = ReadPassword();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty.");
            return false;
        }

        if (choice == "2")
        {
            // Register
            Console.Write("Role (Player/Admin, default: Player): ");
            string? role = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(role))
            {
                role = "Player";
            }

            var (success, message) = await _apiClient!.RegisterAsync(username, password, role);
            Console.WriteLine(message);
            if (!success)
            {
                return false;
            }
            Console.WriteLine("Registration successful! Please login now.");
            return await AuthenticateUser();
        }
        else
        {
            // Login
            var (success, token, user, role, message) = await _apiClient!.LoginAsync(username, password);
            Console.WriteLine(message);
            if (success)
            {
                _userRole = role;
                Console.WriteLine($"Welcome, {user}! Role: {role}");
                return true;
            }
            return false;
        }
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return password;
    }

    static Task HandleEncryptedRoom(Rooms world)
    {
        if (world.CurrentRoom is Room room && room.IsEncrypted)
        {
            Console.WriteLine("\n🔒 This room is encrypted. You need to decrypt it to see its contents.");
            Console.WriteLine("Use 'decrypt <room_name>' command with the correct passphrase.");
        }
        return Task.CompletedTask;
    }

    static async Task HandleDecryptRoom(Rooms world, string roomName)
    {
        Room? room = world.GetRoom(roomName);
        if (room == null)
        {
            Console.WriteLine($"Room '{roomName}' not found.");
            return;
        }

        if (!room.IsEncrypted || string.IsNullOrEmpty(room.EncryptedFilePath))
        {
            Console.WriteLine("This room is not encrypted.");
            return;
        }

        if (string.IsNullOrEmpty(room.RoomId))
        {
            Console.WriteLine("Room ID not configured.");
            return;
        }

        // Get keyshare from API
        var (success, keyshare, message) = await _apiClient!.GetKeyshareAsync(room.RoomId);
        if (!success)
        {
            Console.WriteLine($"Failed to get keyshare: {message}");
            return;
        }

        // Get passphrase from user
        Console.Write("Enter passphrase: ");
        string? passphrase = ReadPassword();

        if (string.IsNullOrWhiteSpace(passphrase))
        {
            Console.WriteLine("Passphrase cannot be empty.");
            return;
        }

        // Decrypt the room
        string? decryptedContent = EncryptionService.DecryptRoomFile(room.EncryptedFilePath, keyshare, passphrase);
        if (decryptedContent == null)
        {
            Console.WriteLine("Decryption failed. Invalid keyshare or passphrase.");
            return;
        }

        // Display decrypted content
        Console.WriteLine("\n" + decryptedContent);
        room.IsEncrypted = false; // Mark as decrypted
    }

    static Task HandleNoclip(Rooms world, string roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
        {
            Console.WriteLine("Usage: noclip <room_name>");
            return Task.CompletedTask;
        }

        Room? targetRoom = world.GetRoom(roomName);
        if (targetRoom == null)
        {
            Console.WriteLine($"Room '{roomName}' not found.");
            return Task.CompletedTask;
        }

        // Admin can enter any room, but encrypted rooms still need decryption
        world.CurrentRoom = targetRoom;
        Console.WriteLine($"\n[NOCLIP] You teleport to {targetRoom.Name}...\n");
        targetRoom.DescribeRoom();

        if (targetRoom.IsEncrypted)
        {
            Console.WriteLine("\n⚠️  Note: This room is encrypted. You can enter it, but you cannot read its contents without decrypting it.");
        }
        return Task.CompletedTask;
    }

    static string SanitizeInput(string input)
    {
        // Remove potentially dangerous characters
        // In a real application, you'd want more sophisticated sanitization
        return input.Replace("\0", "").Trim();
    }

    static void ShowHelp(bool isAdmin)
    {
        Console.WriteLine("\n=== Available Commands ===");
        Console.WriteLine("Movement:");
        Console.WriteLine("  north/n, south/s, east/e, west/w - Move in a direction");
        Console.WriteLine("  up/u, down/d - Move up or down");
        Console.WriteLine("\nActions:");
        Console.WriteLine("  look/l - Examine your surroundings");
        Console.WriteLine("  take [item] - Pick up an item");
        Console.WriteLine("  inventory/inv/i - View your inventory");
        Console.WriteLine("  fight/attack - Fight a monster in the room");
        Console.WriteLine("  status - Show your current status");
        Console.WriteLine("  decrypt <room> - Decrypt an encrypted room");
        if (isAdmin)
        {
            Console.WriteLine("  noclip <room> - Teleport to any room (Admin only)");
        }
        Console.WriteLine("\nOther:");
        Console.WriteLine("  help/h/? - Show this help message");
        Console.WriteLine("  quit/exit/q - Quit the game");
        Console.WriteLine("==========================\n");
    }
}
