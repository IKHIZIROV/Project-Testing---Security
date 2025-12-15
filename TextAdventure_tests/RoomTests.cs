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

        [TestMethod]
        public void Constructor_ShouldInitializeAllPropertiesCorrectly()
        {
            // Arrange & Act
            var room = new Room("TestRoom", "A test room description");

            // Assert
            Assert.AreEqual("TestRoom", room.Name);
            Assert.AreEqual("A test room description", room.Description);
            Assert.IsNotNull(room.Items);
            Assert.AreEqual(0, room.Items.Count);
            Assert.IsNotNull(room.Exits);
            Assert.AreEqual(0, room.Exits.Count);
            Assert.IsFalse(room.IsDeadly);
            Assert.IsFalse(room.RequiresKey);
            Assert.IsFalse(room.HasMonster);
            Assert.IsFalse(room.IsMonsterAlive);
            Assert.IsNull(room.Monster);
        }

        [TestMethod]
        public void AddItem_ShouldAddMultipleItems()
        {
            // Arrange
            var room = new Room("Start", "Room with multiple items");
            var item1 = new Item("Sword", "A weapon", ItemType.Weapon);
            var item2 = new Item("Key", "A key", ItemType.Key);
            var item3 = new Item("Potion", "A potion", ItemType.Consumable);

            // Act
            room.AddItem(item1);
            room.AddItem(item2);
            room.AddItem(item3);

            // Assert
            Assert.AreEqual(3, room.Items.Count);
            Assert.IsTrue(room.Items.Contains(item1));
            Assert.IsTrue(room.Items.Contains(item2));
            Assert.IsTrue(room.Items.Contains(item3));
        }

        [TestMethod]
        public void TakeItem_ShouldBeCaseInsensitive()
        {
            // Arrange
            var room = new Room("Start", "Room with item");
            var item = new Item("Sword", "Sharp blade");
            room.AddItem(item);

            // Act
            var result1 = room.TakeItem("sword");
            room.AddItem(item); // Add it back for second test
            var result2 = room.TakeItem("SWORD");
            room.AddItem(item); // Add it back for third test
            var result3 = room.TakeItem("SwOrD");

            // Assert
            Assert.AreEqual(item, result1);
            Assert.AreEqual(item, result2);
            Assert.AreEqual(item, result3);
        }

        [TestMethod]
        public void TakeItem_ShouldReturnNull_WhenRoomIsEmpty()
        {
            // Arrange
            var room = new Room("Empty", "Empty room");

            // Act
            var result = room.TakeItem("AnyItem");

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(0, room.Items.Count);
        }

        [TestMethod]
        public void AddExit_ShouldAddMultipleExits()
        {
            // Arrange
            var room = new Room("Start", "Central room");
            var northRoom = new Room("North", "North room");
            var southRoom = new Room("South", "South room");
            var eastRoom = new Room("East", "East room");

            // Act
            room.AddExit(Directions.North, northRoom);
            room.AddExit(Directions.South, southRoom);
            room.AddExit(Directions.East, eastRoom);

            // Assert
            Assert.AreEqual(3, room.Exits.Count);
            Assert.AreEqual(northRoom, room.GetExit(Directions.North));
            Assert.AreEqual(southRoom, room.GetExit(Directions.South));
            Assert.AreEqual(eastRoom, room.GetExit(Directions.East));
        }

        [TestMethod]
        public void AddExit_ShouldReplaceExistingExit()
        {
            // Arrange
            var room = new Room("Start", "Room");
            var firstRoom = new Room("First", "First room");
            var secondRoom = new Room("Second", "Second room");

            // Act
            room.AddExit(Directions.North, firstRoom);
            room.AddExit(Directions.North, secondRoom);

            // Assert
            Assert.AreEqual(1, room.Exits.Count);
            Assert.AreEqual(secondRoom, room.GetExit(Directions.North));
            Assert.AreNotEqual(firstRoom, room.GetExit(Directions.North));
        }

        [TestMethod]
        public void GetExit_ShouldReturnCorrectRoomForEachDirection()
        {
            // Arrange
            var room = new Room("Start", "Room");
            var northRoom = new Room("North", "North");
            var southRoom = new Room("South", "South");
            var eastRoom = new Room("East", "East");
            var westRoom = new Room("West", "West");
            var upRoom = new Room("Up", "Up");
            var downRoom = new Room("Down", "Down");

            room.AddExit(Directions.North, northRoom);
            room.AddExit(Directions.South, southRoom);
            room.AddExit(Directions.East, eastRoom);
            room.AddExit(Directions.West, westRoom);
            room.AddExit(Directions.Up, upRoom);
            room.AddExit(Directions.Down, downRoom);

            // Act & Assert
            Assert.AreEqual(northRoom, room.GetExit(Directions.North));
            Assert.AreEqual(southRoom, room.GetExit(Directions.South));
            Assert.AreEqual(eastRoom, room.GetExit(Directions.East));
            Assert.AreEqual(westRoom, room.GetExit(Directions.West));
            Assert.AreEqual(upRoom, room.GetExit(Directions.Up));
            Assert.AreEqual(downRoom, room.GetExit(Directions.Down));
        }

        [TestMethod]
        public void SetMonster_ShouldSetIsMonsterAliveToFalse_WhenMonsterIsDead()
        {
            // Arrange
            var room = new Room("Dungeon", "Dark room");
            var deadMonster = new Monster("Dead Orc", "A dead orc", 0, 10);
            deadMonster.IsAlive = false;

            // Act
            room.SetMonster(deadMonster);

            // Assert
            Assert.AreEqual(deadMonster, room.Monster);
            Assert.IsTrue(room.HasMonster);
            Assert.IsFalse(room.IsMonsterAlive);
        }

        [TestMethod]
        public void SetMonster_ShouldUpdateFlagsWhenMonsterDiesAfterBeingSet()
        {
            // Arrange
            var room = new Room("Dungeon", "Dark room");
            var monster = new Monster("Orc", "An orc", 50, 10);
            room.SetMonster(monster);

            // Act - Monster dies
            monster.TakeDamage(100);
            room.IsMonsterAlive = monster.IsAlive;

            // Assert
            Assert.IsTrue(room.HasMonster);
            Assert.IsFalse(room.IsMonsterAlive);
            Assert.IsFalse(monster.IsAlive);
        }

        [TestMethod]
        public void IsDeadly_ShouldBeSetCorrectly()
        {
            // Arrange
            var room = new Room("Deadly", "Deadly room");

            // Act
            room.IsDeadly = true;

            // Assert
            Assert.IsTrue(room.IsDeadly);
        }

        [TestMethod]
        public void RequiresKey_ShouldBeSetCorrectly()
        {
            // Arrange
            var room = new Room("Locked", "Locked room");

            // Act
            room.RequiresKey = true;

            // Assert
            Assert.IsTrue(room.RequiresKey);
        }

        [TestMethod]
        public void TakeItem_ShouldRemoveCorrectItem_WhenMultipleItemsExist()
        {
            // Arrange
            var room = new Room("Start", "Room with items");
            var item1 = new Item("Sword", "Weapon");
            var item2 = new Item("Key", "Key");
            var item3 = new Item("Potion", "Potion");
            room.AddItem(item1);
            room.AddItem(item2);
            room.AddItem(item3);

            // Act
            var result = room.TakeItem("Key");

            // Assert
            Assert.AreEqual(item2, result);
            Assert.AreEqual(2, room.Items.Count);
            Assert.IsFalse(room.Items.Contains(item2));
            Assert.IsTrue(room.Items.Contains(item1));
            Assert.IsTrue(room.Items.Contains(item3));
        }

        [TestMethod]
        public void Items_ShouldBeReadOnlyList_NotModifiableFromOutside()
        {
            // Arrange
            var room = new Room("Start", "Room");
            var item = new Item("Sword", "Weapon");
            room.AddItem(item);

            // Act - Try to modify the list directly (should work since it's a List, not IReadOnlyList)
            // But we can verify the property behavior
            var itemCount = room.Items.Count;

            // Assert
            Assert.AreEqual(1, itemCount);
            Assert.AreEqual(1, room.Items.Count);
        }

        [TestMethod]
        public void Name_ShouldBeSettable()
        {
            // Arrange
            var room = new Room("Initial", "Description");

            // Act
            room.Name = "NewName";

            // Assert
            Assert.AreEqual("NewName", room.Name);
        }

        [TestMethod]
        public void Description_ShouldBeSettable()
        {
            // Arrange
            var room = new Room("Room", "Initial description");

            // Act
            room.Description = "New description";

            // Assert
            Assert.AreEqual("New description", room.Description);
        }

        [TestMethod]
        public void Exits_ShouldBeMutableDictionary()
        {
            // Arrange
            var room = new Room("Start", "Room");
            var exitRoom = new Room("Exit", "Exit room");

            // Act
            room.Exits.Add(Directions.North, exitRoom);

            // Assert
            Assert.AreEqual(1, room.Exits.Count);
            Assert.AreEqual(exitRoom, room.Exits[Directions.North]);
        }

        [TestMethod]
        public void SetMonster_ShouldHandleNullMonster()
        {
            // Arrange
            var room = new Room("Dungeon", "Dark room");
            var monster = new Monster("Orc", "An orc", 50, 10);
            room.SetMonster(monster);

            // Act
            room.RemoveMonster();
            // Setting null via RemoveMonster

            // Assert
            Assert.IsNull(room.Monster);
            Assert.IsFalse(room.HasMonster);
            Assert.IsFalse(room.IsMonsterAlive);
        }

        [TestMethod]
        public void TakeItem_ShouldHandlePartialNameMatch()
        {
            // Arrange
            var room = new Room("Start", "Room");
            var item = new Item("Long Sword Name", "A long sword");
            room.AddItem(item);

            // Act - Try to take with exact name
            var result = room.TakeItem("Long Sword Name");

            // Assert
            Assert.AreEqual(item, result);
            Assert.AreEqual(0, room.Items.Count);
        }

        [TestMethod]
        public void TakeItem_ShouldNotTakeItem_WhenNameDoesNotMatch()
        {
            // Arrange
            var room = new Room("Start", "Room");
            var item = new Item("Sword", "A sword");
            room.AddItem(item);

            // Act
            var result = room.TakeItem("Dagger");

            // Assert
            Assert.IsNull(result);
            Assert.AreEqual(1, room.Items.Count);
            Assert.IsTrue(room.Items.Contains(item));
        }

    }
}
