using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ItemType Type { get; set; }
    public int Value { get; set; } // For weapons: damage, for keys: which door it opens

    public Item(string name, string description, ItemType type = ItemType.Generic, int value = 0)
    {
        Name = name;
        Description = description;
        Type = type;
        Value = value;
    }

    public bool IsWeapon()
    {
        return Type == ItemType.Weapon;
    }
    public bool IsKey()
    {
        return Type == ItemType.Key;
    }
    public override string ToString()
    {
        return $"{Name} - {Description}";
    }
}

public enum ItemType
{
    Generic,
    Weapon,
    Key,
    Consumable,
    Treasure
}
