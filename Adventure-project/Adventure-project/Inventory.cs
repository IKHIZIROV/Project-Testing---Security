using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project;

public class Inventory
{
    private List<Item> items = new List<Item>();
    public int MaxCapacity { get; set; } = 20; // Optional: limit inventory size

    public Inventory()
    {
        items = new List<Item>();
    }
    public void AddItem(Item item)
    {
        if (items.Count >= MaxCapacity)
        {
            Console.WriteLine("Your inventory is full!");
            return;
        }
        items.Add(item);
        Console.WriteLine($"Added {item.Name} to inventory.");
    }
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Console.WriteLine($"Removed {item.Name} from inventory.");
        }
        else
        {
            Console.WriteLine("Item not found in inventory.");
        }
    }
    public bool HasItem(string itemName)
    {
        return items.Any(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
    }
    public Item GetItem(string itemName)
    {
        return items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
    }
    public bool HasWeapon()
    {
        return items.Any(i => i.IsWeapon());
    }
    public Item GetBestWeapon()
    {
        return items.Where(i => i.IsWeapon())
                    .OrderByDescending(i => i.Value)
                    .FirstOrDefault();
    }
    public bool HasKey()
    {
        return items.Any(i => i.IsKey());
    }
    public void ShowInventory()
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
            return;
        }

        Console.WriteLine("\n=== Your Inventory ===");
        foreach (var item in items)
        {
            string itemInfo = $"- {item.Name}: {item.Description}";
            if (item.IsWeapon())
            {
                itemInfo += $" (Damage: {item.Value})";
            }
            Console.WriteLine(itemInfo);
        }
        Console.WriteLine($"Items: {items.Count}/{MaxCapacity}\n");
    }
    public int GetItemCount()
    {
        return items.Count;
    }
    public List<Item> GetAllItems()
    {
        return new List<Item>(items); // Return a copy to prevent external modification
    }
}
