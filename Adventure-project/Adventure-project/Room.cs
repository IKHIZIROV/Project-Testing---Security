using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project
{
    public class Room : Rooms
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }

        public Dictionary<Directions, Rooms> Exits { get; set; } = new Dictionary<Directions, Rooms>();

        // veranderd naar een property en public gemaakt
        public List<Item> Items { get; }

        public bool IsDeadly { get; set; }

        public bool RequiresKey { get; set; }

        public bool HasMonster { get; set; }

        public bool IsMonsterAlive { get; set; }

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
        }

        public void DescribeRoom()
        {
            Console.WriteLine(Name);
            Console.WriteLine(Description);
            ShowItems();
        }

        public void ShowItems()
        {
            if (Items.Count == 0)
            {
                Console.WriteLine("There are no items in this room.");
            }
            else
            {
                Console.WriteLine("Items in the room:");
                foreach (var item in Items)
                {
                    Console.WriteLine($"- {item.Name}: {item.Description}");
                }
            }
        }

        public Item TakeItem(string itemId)
        {
            Item itemToTake = Items.FirstOrDefault(i => i.Name.Equals(itemId, StringComparison.OrdinalIgnoreCase));
            if (itemToTake != null)
            {
                Items.Remove(itemToTake);
            }
            return itemToTake;
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

        public void GetExit(Directions direction)
        {
            if (Exits.ContainsKey(direction))
            {
                Rooms nextRoom = Exits[direction];
                Console.WriteLine($"You move {direction} to {nextRoom.Name}.");
            }
            else
            {
                Console.WriteLine("There is no exit in that direction.");
            }
        }
    }
}
