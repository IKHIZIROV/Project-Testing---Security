namespace Adventure_project;

public enum Directions
{
    North, East, South, West, Up, Down
}

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("       WELCOME TO THE CASTLE ADVENTURE!         ");
        Console.WriteLine("=================================================\n");

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
            string input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            string[] parts = input.Split(' ', 2);
            string command = parts[0];
            string argument = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "north":
                case "n":
                    world.Move(Directions.North);
                    break;

                case "south":
                case "s":
                    world.Move(Directions.South);
                    break;

                case "east":
                case "e":
                    world.Move(Directions.East);
                    break;

                case "west":
                case "w":
                    world.Move(Directions.West);
                    break;

                case "up":
                case "u":
                    world.Move(Directions.Up);
                    break;

                case "down":
                case "d":
                    world.Move(Directions.Down);
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
                    if (string.IsNullOrEmpty(argument))
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

                case "help":
                case "h":
                case "?":
                    ShowHelp();
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
                string playAgain = Console.ReadLine()?.ToLower();
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
    static void ShowHelp()
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
        Console.WriteLine("\nOther:");
        Console.WriteLine("  help/h/? - Show this help message");
        Console.WriteLine("  quit/exit/q - Quit the game");
        Console.WriteLine("==========================\n");
    }
}