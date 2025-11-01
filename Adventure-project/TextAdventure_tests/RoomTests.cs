using Adventure_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_tests
{
    [TestClass]
    public sealed class RoomTests
    {
        [TestMethod]
        public void AddItem_IncreaseCount()
        {
            // Arrange 
            var room = new Room("Start", "A small room.");
            var item = new Item("Key", "Opens a door.");

            // Act 
            room.AddItem(item);

            // Assert
            Assert.AreEqual(1, room.Items.Count);
            Assert.AreEqual("Key", room.Items[0].Name);
        }
        [TestMethod]
        public void TakeItem_RemoveItemAndReturnIt()
        {
            // Arrange 
            var room = new Room("Start", "Room with item");
            var item = new Item("Sword", "Sharp blade");
            room.AddItem(item);
            // Act 
            var result = room.TakeItem("Sword");

            // Assert 
            Assert.AreEqual(item, result);
            Assert.AreEqual(0, room.Items.Count);

        }
        [TestMethod]
        public void TakeItem_ReturnNull_WhenItemNotFound()
        {
            // Arrange 
            var room = new Room("Start", "Empty room");

            // Act 
            var result = room.TakeItem("Nonexistent");

            // Assert 
            Assert.IsNull(result);
        }
        [TestMethod]
        public void AddExit_ShouldAddConnectionToAnotherRoom()
        {
            // Arrange 
            var room = new Room("Start", "Main room");
            var nextRoom = new Room("Next", "Second room");

            // Act 
            room.AddExit(Directions.North, nextRoom);

            // Assert 
            Assert.AreEqual(nextRoom, room.GetExit(Directions.North));
        }
        [TestMethod]
        public void GetExit_ShouldReturnNull_WhenNoExitInThatDirection()
        {
            // Arrange 
            var room = new Room("Start", "Room");

            // Act 
            var result = room.GetExit(Directions.South);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void SetMonster_ShouldAssignMonsterAndFlags()
        {
            // Arrange 
            var room = new Room("Dungeon", "Dark room");
            var monster = new Monster("Big Juicy Orc", "A big Orc that can molest you.", 20, 15);

            // Act 
            room.SetMonster(monster);

            // Assert 
            Assert.AreEqual(monster, room.Monster);
            Assert.IsTrue(room.HasMonster);
            Assert.IsTrue(room.IsMonsterAlive);
        }
        [TestMethod]
        public void RemoveMonster_ShouldClearMonsterAndFlags()
        {
            // Arrange
            var room = new Room("Dungeon", "Dark room");
            var monster = new Monster("Goblin gang", "A group of 6 goblins", 10, 5);
            room.SetMonster(monster);

            // Act 
            room.RemoveMonster();

            // Assert 
            Assert.IsNull(room.Monster);
            Assert.IsFalse(room.HasMonster);
            Assert.IsFalse(room.IsMonsterAlive);
        }

    }
}
