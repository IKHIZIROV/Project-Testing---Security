using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project
{
    public class Inventory
    {
        List<Item> items = new List<Item>();

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public void HasItem(Item item)
        {
            items.Contains(item);
        }

        public void ShowInventory()
        {
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name} - {item.Description}");
            }
        }
    }
}
