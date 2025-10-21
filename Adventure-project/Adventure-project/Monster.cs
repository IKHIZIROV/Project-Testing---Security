using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project;

public class Monster
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int DefenseRating { get; set; }
    public bool IsAlive { get; set; }
    public List<Item> LootTable { get; set; } // Items dropped when defeated
    public MonsterType Type { get; set; }

    public Monster(string name, string description, int health, int attackPower, int defenseRating = 0, MonsterType type = MonsterType.Beast)
    {
        Name = name;
        Description = description;
        Health = health;
        AttackPower = attackPower;
        DefenseRating = defenseRating;
        IsAlive = true;
        LootTable = new List<Item>();
        Type = type;
    }

    public void TakeDamage(int damage)
    {
        int actualDamage = Math.Max(1, damage - DefenseRating); // Minimum 1 damage
        Health -= actualDamage;
        Console.WriteLine($"{Name} takes {actualDamage} damage! (Health: {Math.Max(0, Health)}/{Health + actualDamage})");

        if (Health <= 0)
        {
            Health = 0;
            IsAlive = false;
            Console.WriteLine($"{Name} has been defeated!");
        }
    }
    public int Attack()
    {
        if (!IsAlive)
        {
            return 0;
        }

        Random random = new Random();
        int damage = random.Next(AttackPower / 2, AttackPower + 1); // Random damage between half and full attack power
        Console.WriteLine($"{Name} attacks for {damage} damage!");
        return damage;
    }
    public void DisplayInfo()
    {
        Console.WriteLine($"\n=== {Name} ===");
        Console.WriteLine(Description);
        Console.WriteLine($"Health: {Health}");
        Console.WriteLine($"Attack Power: {AttackPower}");
        Console.WriteLine($"Defense: {DefenseRating}");
        Console.WriteLine($"Status: {(IsAlive ? "Alive" : "Defeated")}\n");
    }
    public void AddLoot(Item item)
    {
        LootTable.Add(item);
    }
    public List<Item> DropLoot()
    {
        if (!IsAlive && LootTable.Count > 0)
        {
            Console.WriteLine($"{Name} dropped some items!");
            return new List<Item>(LootTable);
        }
        return new List<Item>();
    }

    // Factory method to create preset monsters
    public static Monster CreateGoblin()
    {
        return new Monster("Goblin", "A small, vicious creature with sharp claws.", 30, 8, 2, MonsterType.Goblinoid);
    }
    public static Monster CreateOrc()
    {
        return new Monster("Orc", "A large, brutish warrior wielding a rusty axe.", 50, 12, 5, MonsterType.Goblinoid);
    }
    public static Monster CreateDragon()
    {
        var dragon = new Monster("Dragon", "A massive winged beast with scales of fire.", 100, 20, 10, MonsterType.Dragon);
        dragon.AddLoot(new Item("Dragon Scale", "A shimmering scale from a mighty dragon.", ItemType.Treasure, 100));
        return dragon;
    }
    public static Monster CreateSkeleton()
    {
        return new Monster("Skeleton", "An animated pile of bones rattling ominously.", 25, 10, 3, MonsterType.Undead);
    }
    public static Monster CreateTroll()
    {
        return new Monster("Troll", "A huge, regenerating beast with incredible strength.", 70, 15, 8, MonsterType.Beast);
    }
}

public enum MonsterType
{
    Beast,
    Undead,
    Goblinoid,
    Dragon,
    Demon,
    Elemental
}
