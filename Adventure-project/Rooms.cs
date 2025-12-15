using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project;

public class Rooms
{
    public object Name { get; internal set; }
    public Room CurrentRoom { get; set; }
    public Dictionary<string, Room> AllRooms { get; set; }
    public Inventory PlayerInv { get; }
    public int PlayerHealth { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
    public bool GameOver { get; set; }
    public bool GameWon { get; set; }

    // Dictionary to store monsters in rooms
    public Dictionary<string, Monster> RoomMonsters { get; set; }

    public Rooms()
    {
        AllRooms = new Dictionary<string, Room>();
        PlayerInv = new Inventory();
        RoomMonsters = new Dictionary<string, Monster>();
        GameOver = false;
        GameWon = false;
    }
    public void AddRoom(Room room)
    {
        AllRooms[room.Name.ToLower()] = room;
    }
    public Room GetRoom(string roomName)
    {
        if (AllRooms.ContainsKey(roomName.ToLower()))
        {
            return AllRooms[roomName.ToLower()];
        }
        return null;
    }
    public void SetSpawnRoom(string roomName)
    {
        Room startRoom = GetRoom(roomName.ToLower());
        if (startRoom != null)
        {
            CurrentRoom = startRoom;
            CurrentRoom.DescribeRoom();
        }
        else
        {
            Console.WriteLine($"Start room '{roomName}' not found!");
        }
    }
    public void AddMonsterToRoom(string roomName, Monster monster)
    {
        string key = roomName.ToLower();
        if (AllRooms.ContainsKey(key))
        {
            RoomMonsters[key] = monster;
            AllRooms[key].HasMonster = true;
            AllRooms[key].IsMonsterAlive = monster.IsAlive;
        }
    }
    public void Move(Directions direction)
    {
        if (CurrentRoom.Exits.ContainsKey(direction))
        {
            Room nextRoom = (Room)CurrentRoom.Exits[direction];

            // Check for deadly room
            if (nextRoom.IsDeadly)
            {
                Console.WriteLine("You enter the room and immediately feel the effects of poison gas!");
                Console.WriteLine("You take 50 damage!");
                PlayerHealth -= 50;
                CheckPlayerDeath();
                if (GameOver) return;
            }

            // Check for living monster blocking the path
            if (nextRoom.HasMonster && nextRoom.IsMonsterAlive)
            {
                Console.WriteLine($"A monster blocks your path to the {nextRoom.Name}!");
                Console.WriteLine("You must defeat it before proceeding. Use 'fight' command.");
                return;
            }

            // Check for locked door
            if (nextRoom.RequiresKey)
            {
                if (PlayerInv.HasKey())
                {
                    Console.WriteLine("You use your key to unlock the door!");
                    nextRoom.RequiresKey = false; // Unlock permanently
                }
                else
                {
                    Console.WriteLine("The door is locked. You need a key to enter this room.");
                    return;
                }
            }

            // Move to the next room
            CurrentRoom = nextRoom;
            Console.WriteLine($"\nYou move {direction}...\n");
            
            // Check if room is encrypted
            if (nextRoom.IsEncrypted)
            {
                Console.WriteLine($"\n=== {nextRoom.Name} ===");
                Console.WriteLine("🔒 This room is encrypted. The contents are hidden.");
                Console.WriteLine("Use 'decrypt " + nextRoom.Name.ToLower() + "' to decrypt it.");
            }
            else
            {
                CurrentRoom.DescribeRoom();
            }

            // Check if this is the winning room
            if (CurrentRoom.Name == "Win")
            {
                GameWon = true;
                GameOver = true;
                Console.WriteLine("\n🎉 CONGRATULATIONS! YOU WON THE GAME! 🎉\n");
            }
        }
        else
        {
            Console.WriteLine("You can't go that way.");
        }
    }
    public void FightMonster()
    {
        if (!CurrentRoom.HasMonster || !CurrentRoom.IsMonsterAlive)
        {
            Console.WriteLine("There is no monster to fight in this room.");
            return;
        }

        string roomKey = CurrentRoom.Name.ToLower();
        if (!RoomMonsters.ContainsKey(roomKey))
        {
            Console.WriteLine("Monster data not found.");
            return;
        }

        Monster monster = RoomMonsters[roomKey];

        if (!monster.IsAlive)
        {
            Console.WriteLine("The monster is already defeated.");
            return;
        }

        // Check if player has a weapon
        if (!PlayerInv.HasWeapon())
        {
            Console.WriteLine("You have no weapon! You cannot fight bare-handed.");
            Console.WriteLine("Find a weapon first!");
            return;
        }

        Console.WriteLine($"\n⚔️  COMBAT INITIATED ⚔️");
        monster.DisplayInfo();

        Item weapon = PlayerInv.GetBestWeapon();
        Console.WriteLine($"You wield your {weapon.Name}!");

        // Combat loop
        while (monster.IsAlive && PlayerHealth > 0)
        {
            Console.WriteLine("\n--- Your Turn ---");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Run Away");
            Console.Write("Choose action: ");

            string choice = Console.ReadLine();

            if (choice == "2")
            {
                Console.WriteLine("You flee from combat!");
                return;
            }

            // Player attacks
            Console.WriteLine($"You swing your {weapon.Name}!");
            monster.TakeDamage(weapon.Value);

            if (!monster.IsAlive)
            {
                Console.WriteLine($"\n🗡️  Victory! You have defeated the {monster.Name}! 🗡️\n");
                CurrentRoom.IsMonsterAlive = false;

                // Drop loot
                List<Item> loot = monster.DropLoot();
                foreach (Item item in loot)
                {
                    CurrentRoom.AddItem(item);
                    Console.WriteLine($"The monster dropped: {item.Name}");
                }
                break;
            }

            // Monster attacks
            Console.WriteLine("\n--- Monster's Turn ---");
            int damage = monster.Attack();
            PlayerHealth -= damage;
            Console.WriteLine($"Your health: {PlayerHealth}/{MaxHealth}");

            CheckPlayerDeath();
            if (GameOver) return;
        }
    }
    private void CheckPlayerDeath()
    {
        if (PlayerHealth <= 0)
        {
            PlayerHealth = 0;
            GameOver = true;
            Console.WriteLine("\n💀 YOU HAVE DIED! GAME OVER! 💀\n");
        }
    }
    public void TakeItem(string itemId)
    {
        Item item = CurrentRoom.TakeItem(itemId);
        if (item != null)
        {
            PlayerInv.AddItem(item);
        }
        else
        {
            Console.WriteLine("Item not found in this room.");
        }
    }
    public void Look()
    {
        Console.WriteLine("\n");
        CurrentRoom.DescribeRoom();

        // Show available exits
        if (CurrentRoom.Exits.Count > 0)
        {
            Console.WriteLine("\nAvailable exits:");
            foreach (var exit in CurrentRoom.Exits.Keys)
            {
                Console.WriteLine($"- {exit}");
            }
        }

        // Show monster status
        if (CurrentRoom.HasMonster)
        {
            string roomKey = CurrentRoom.Name.ToLower();
            if (RoomMonsters.ContainsKey(roomKey))
            {
                Monster monster = RoomMonsters[roomKey];
                if (monster.IsAlive)
                {
                    Console.WriteLine($"\n⚠️  WARNING: {monster.Name} is here!");
                }
                else
                {
                    Console.WriteLine($"\nThe defeated {monster.Name} lies here.");
                }
            }
        }

        Console.WriteLine($"\nYour Health: {PlayerHealth}/{MaxHealth}");
    }
    public void ShowStatus()
    {
        Console.WriteLine("\n=== Status ===");
        Console.WriteLine($"Current Room: {CurrentRoom.Name}");
        Console.WriteLine($"Health: {PlayerHealth}/{MaxHealth}");
        Console.WriteLine($"Inventory Items: {PlayerInv.GetItemCount()}");
    }
}
