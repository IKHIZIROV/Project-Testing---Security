using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project
{
    public class Rooms
    {
        // dit is niet nodig
        //public object Name { get; internal set; }
        public Room CurrentRoom { get; set; }
        public Dictionary<string, Room> AllRooms { get; set; }
        public Inventory PlayerInv { get; } // player inventory
        public bool GameOver { get; set; } // bools om te checken of de speler de game heeft gewonnen of niet
        public bool GameWon { get; set; }

        public Rooms()
        {
            AllRooms = new Dictionary<string, Room>();
            PlayerInv = new Inventory();
            GameOver = false;
            GameWon = false;
        }
        //add room methode toegevoegd
        public void AddRoom(Room room)
        {
            AllRooms[room.Name.ToLower()] = room;
        }
        // getroom methode toegevoegd
        public Room GetRoom(string roomName)
        {
            if (AllRooms.ContainsKey(roomName.ToLower()))
            {
                return AllRooms[roomName.ToLower()];
            }
            return null;
        }
        // methode voor start kamer
        public void SetSpawnRoom(string roomName)
        {
            Room startRoom = GetRoom(roomName.ToLower());
            if (startRoom != null)
            {
                CurrentRoom = startRoom;
            }
            else
            {
                Console.WriteLine($"Start room '{roomName}' not found!");
            }
        }
        public void Move(Directions direction)
        {
            if (CurrentRoom.Exits.ContainsKey(direction))
            {
                if (CurrentRoom.HasMonster)
                {
                    Console.WriteLine("This room has a monster!");
                    return;

                } else if (CurrentRoom.RequiresKey) 
                {
                    Console.WriteLine("You need a key to enter this room.");
                    return;

                } else
                {
                    CurrentRoom = (Room)CurrentRoom.Exits[direction];
                    CurrentRoom.DescribeRoom();
                }
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }

        public void FightMonster()
        {
            if (CurrentRoom.HasMonster && CurrentRoom.IsMonsterAlive)
            {
                Console.WriteLine("You engage in a fight with the monster!");
                
            }
            else
            {
                Console.WriteLine("There is no monster to fight in this room.");
            }
        }

        public void TakeItem(string itemId)
        {
            Item item = CurrentRoom.TakeItem(itemId);
            if (item != null)
            {
                Console.WriteLine($"You have taken the {item.Name}.");
            }
            else
            {
                Console.WriteLine("Item not found in this room.");
            }
        }
        // look methode toegevoegd
        public void Look()
        {
            CurrentRoom.DescribeRoom();
            //Console.WriteLine(PlayerInv.ShowInventory());
        }
    }
}
