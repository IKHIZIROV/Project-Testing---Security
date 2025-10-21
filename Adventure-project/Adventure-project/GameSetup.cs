using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project;

public class GameSetup
{
    public Rooms CreateWorld()
    {
        // Create the world
        Rooms world = new Rooms();

        // Create rooms
        Room start = new Room("Start", "You are in the middle of the Castle. The grand hall echoes with silence. You see exits to the North, East, South, and West.");

        Room west = new Room("West", "A very strange room filled with poisonous gas. You can't stay here long!")
        {
            IsDeadly = true
        };

        Room east = new Room("East", "A treasury room with golden walls. A shiny key lies on a pedestal.");

        Room north = new Room("North", "A heavy locked door blocks your path. You need a key to proceed.")
        {
            RequiresKey = true
        };

        Room south = new Room("South", "An armory filled with ancient weapons. A magnificent sword rests on a stone altar.");

        Room deeper = new Room("Deeper", "A dark chamber. You hear growling... A fierce Orc guards this room!")
        {
            HasMonster = true,
            IsMonsterAlive = true
        };

        Room win = new Room("Win", "You have reached the throne room and claimed victory! Congratulations, brave adventurer!");

        // Create items
        Item key = new Item("Key", "A golden key that can open locked doors", ItemType.Key, 1);
        Item sword = new Item("Sword", "A sharp blade that deals 15 damage to enemies", ItemType.Weapon, 15);
        Item dagger = new Item("Dagger", "A small but deadly weapon dealing 8 damage", ItemType.Weapon, 8);
        Item potion = new Item("Potion", "A healing potion that restores health", ItemType.Consumable, 20);

        // Add items to rooms
        east.AddItem(key);
        south.AddItem(sword);
        start.AddItem(dagger);

        // Create monster for the deeper room
        Monster orc = Monster.CreateOrc();
        orc.AddLoot(potion);
        // Note: You'll need to add a Monster property to Room class or handle it differently
        // For now, the HasMonster and IsMonsterAlive flags are set

        // Connect rooms - Start room
        start.AddExit(Directions.North, north);
        start.AddExit(Directions.East, east);
        start.AddExit(Directions.South, south);
        start.AddExit(Directions.West, west);

        // Connect rooms - East
        east.AddExit(Directions.West, start);

        // Connect rooms - South
        south.AddExit(Directions.North, start);

        // Connect rooms - West (deadly room - maybe leads back)
        west.AddExit(Directions.East, start);

        // Connect rooms - North (requires key)
        north.AddExit(Directions.South, start);
        north.AddExit(Directions.Down, deeper);

        // Connect rooms - Deeper (monster room)
        deeper.AddExit(Directions.Up, north);
        deeper.AddExit(Directions.North, win);

        // Connect rooms - Win room
        win.AddExit(Directions.South, deeper);

        // Add all rooms to the world
        world.AddRoom(start);
        world.AddRoom(east);
        world.AddRoom(south);
        world.AddRoom(west);
        world.AddRoom(north);
        world.AddRoom(deeper);
        world.AddRoom(win);

        // Set the starting room
        world.SetSpawnRoom("Start");

        Console.WriteLine("=== Welcome to Castle Adventure! ===");
        Console.WriteLine("Your goal: Navigate through the castle, defeat monsters, and reach the throne room!");
        Console.WriteLine("Commands: north, south, east, west, up, down, look, inventory, take [item], fight\n");

        return world;
    }

    // Alternative: Create a more complex world
    public Rooms CreateAdvancedWorld()
    {
        Rooms world = new Rooms();

        // Create multiple rooms with various challenges
        Room entrance = new Room("Castle Entrance", "You stand before a massive castle. Stone walls tower above you.");
        Room hallway = new Room("Grand Hallway", "A long corridor with portraits of ancient kings.");
        Room library = new Room("Library", "Dusty books line the shelves. Knowledge awaits.");
        Room dungeon = new Room("Dungeon", "A dark, damp prison. Chains hang from the walls.")
        {
            HasMonster = true,
            IsMonsterAlive = true
        };
        Room treasureRoom = new Room("Treasure Room", "Gold and jewels sparkle in the torchlight!")
        {
            RequiresKey = true
        };
        Room dragonLair = new Room("Dragon's Lair", "A massive cave. The air is hot and smoky.")
        {
            HasMonster = true,
            IsMonsterAlive = true
        };
        Room throneRoom = new Room("Throne Room", "You've reached the king's throne. Victory is yours!");

        // Create items
        Item rustySword = new Item("Rusty Sword", "An old sword, but still useful", ItemType.Weapon, 10);
        Item magicSword = new Item("Magic Sword", "A blade infused with ancient magic", ItemType.Weapon, 25);
        Item dungeonKey = new Item("Dungeon Key", "Opens the treasure room", ItemType.Key, 1);
        Item healthPotion = new Item("Health Potion", "Restores 30 health points", ItemType.Consumable, 30);
        Item crown = new Item("Crown", "The king's golden crown", ItemType.Treasure, 1000);

        // Add items to rooms
        entrance.AddItem(rustySword);
        library.AddItem(healthPotion);
        dungeon.AddItem(dungeonKey);
        treasureRoom.AddItem(magicSword);
        treasureRoom.AddItem(crown);

        // Connect rooms
        entrance.AddExit(Directions.North, hallway);
        hallway.AddExit(Directions.South, entrance);
        hallway.AddExit(Directions.East, library);
        hallway.AddExit(Directions.West, dungeon);
        hallway.AddExit(Directions.North, treasureRoom);
        library.AddExit(Directions.West, hallway);
        dungeon.AddExit(Directions.East, hallway);
        treasureRoom.AddExit(Directions.South, hallway);
        treasureRoom.AddExit(Directions.Up, dragonLair);
        dragonLair.AddExit(Directions.Down, treasureRoom);
        dragonLair.AddExit(Directions.North, throneRoom);
        throneRoom.AddExit(Directions.South, dragonLair);

        // Add rooms to world
        world.AddRoom(entrance);
        world.AddRoom(hallway);
        world.AddRoom(library);
        world.AddRoom(dungeon);
        world.AddRoom(treasureRoom);
        world.AddRoom(dragonLair);
        world.AddRoom(throneRoom);

        world.SetSpawnRoom("Castle Entrance");

        return world;
    }
}
