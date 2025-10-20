using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Adventure_project
{
    public class GameSetup
    {
        public Rooms Createworld()
        {
            // lijst van kamers
            Rooms world = new Rooms();

            //kamers
            Room start = new Room("Start", "You are in the middle of the Castle");
            Room west = new Room("West", "A very strange room...") { IsDeadly = true };
            Room east = new Room("East", "A room with an object on the ground.");
            Room north = new Room("North", "A locked door. Maybe I need a key.") { RequiresKey = true };
            Room south = new Room("South", "A room with a sharp object on the ground.");
            Room deeper = new Room("Deeper", "A scary room with a monster!") { HasMonster = true, IsMonsterAlive = true };
            Room win = new Room("Win", "You have won!") { GameWon = true };

            east.AddItem(new Item("Key", "Can be used to open a door"));
            south.AddItem(new Item("Sword", "A sharp object to slay monsters"));
            
            
            return world;
        }

        

    }
}
