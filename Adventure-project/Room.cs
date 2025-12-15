using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project;

public class Room : Rooms
{
    public new string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    public Dictionary<Directions, Rooms> Exits { get; set; } = new Dictionary<Directions, Rooms>();

    // veranderd naar een property en public gemaakt
    public List<Item> Items { get; }
    public bool IsDeadly { get; set; }
    public bool RequiresKey { get; set; }
    public bool HasMonster { get; set; }
    public bool IsMonsterAlive { get; set; }
    public Monster Monster { get; set; }
    public bool IsEncrypted { get; set; }
    public string? EncryptedFilePath { get; set; }
    public string? RoomId { get; set; } // Used for keyshare lookup

    // constructor aangemaakt
    public Room(string name, string description)
    {
        Name = name;
        Description = description;
        Exits = new Dictionary<Directions, Rooms>();
        Items = new List<Item>();
        IsDeadly = false;
        RequiresKey = false ;
        HasMonster = false;
        IsMonsterAlive = false ;
        Monster = null;
        IsEncrypted = false;
        EncryptedFilePath = null;
        RoomId = null;
    }

    public void DescribeRoom()
    {
        Console.WriteLine($"\n=== {Name} ===");
        Console.WriteLine(Description);
        ShowItems();
        ShowMonster();
        ShowWarnings();
    }
    public void ShowItems()
    {
        if (Items.Count == 0)
        {
            Console.WriteLine("\nThere are no items in this room.");
        }
        else
        {
            Console.WriteLine("\nItems in the room:");
            foreach (var item in Items)
            {
                string itemType = "";
                if (item.IsWeapon())
                {
                    itemType = $" [Weapon - Damage: {item.Value}]";
                }
                else if (item.IsKey())
                {
                    itemType = " [Key]";
                }
                Console.WriteLine($"  • {item.Name}: {item.Description}{itemType}");
            }
        }
    }
    public void ShowMonster()
    {
        if (HasMonster && Monster != null)
        {
            if (Monster.IsAlive)
            {
                Console.WriteLine($"\n⚠️  WARNING: A {Monster.Name} is here!");
                Console.WriteLine($"   {Monster.Description}");
            }
            else
            {
                Console.WriteLine($"\nThe defeated {Monster.Name} lies motionless on the ground.");
            }
        }
    }
    public void ShowWarnings()
    {
        if (IsDeadly)
        {
            Console.WriteLine("\n⚠️  DANGER: This room is deadly!");
        }
        if (RequiresKey)
        {
            Console.WriteLine("\n🔒 This room is locked. You need a key.");
        }
    }
    public new Item TakeItem(string itemId)
    {
        Item itemToTake = Items.FirstOrDefault(i => i.Name.Equals(itemId, StringComparison.OrdinalIgnoreCase));
        if (itemToTake != null)
        {
            Items.Remove(itemToTake);
            return itemToTake;
        }
        return null;
    }
    // add item methode toegevoegd
    public void AddItem(Item item)
    {
        Items.Add(item);
    }
    public void AddExit(Directions direction, Rooms room)
    {
        Exits[direction] = room;
    }
    public Rooms GetExit(Directions direction)
    {
        if (Exits.ContainsKey(direction))
        {
            return Exits[direction];
        }
        return null;
    }
    public void SetMonster(Monster monster)
    {
        Monster = monster;
        HasMonster = true;
        IsMonsterAlive = monster.IsAlive;
    }
    public void RemoveMonster()
    {
        Monster = null;
        HasMonster = false;
        IsMonsterAlive = false;
    }
}
