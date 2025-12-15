using Adventure_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_tests
{
    [TestClass]
    public sealed class InventoryTests
    {
        [TestMethod]
        public void AddItem_shouldIncreaseCount()
        {
            // Arrange
            var inventory = new Inventory();
            var item = new Item("Sword", "A sharp blade.", ItemType.Weapon);

            // Act 
            inventory.AddItem(item);

            // Assert 
            Assert.AreEqual(1, inventory.GetItemCount());
            Assert.IsTrue(inventory.HasItem("Sword"));
        }

        [TestMethod]
        public void RemoveItem_shouldRemoveExistingItem()
        {
            // Arrange 
            var inventory = new Inventory();
            var item = new Item("Key", "Opens a door.", ItemType.Key);
            inventory.AddItem(item);

            // Act 
            inventory.RemoveItem(item);

            // Assert
            Assert.IsFalse(inventory.HasItem("Key"));
            Assert.AreEqual(0, inventory.GetItemCount());
        }

        [TestMethod]
        public void HasItem_ShouldReturnTrue()
        {
            // Arrange 
            var inventory = new Inventory();
            var item = new Item("Potion", "Restores health.");
            inventory.AddItem(item);

            // Act 
            bool result = inventory.HasItem("potion");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetItem_ReturnCorrectItemByName()
        {
            // Arrange
            var inventory = new Inventory();
            var item = new Item("Shield", "Protects you.");
            inventory.AddItem(item);

            // Act 
            var result = inventory.GetItem("Shield");

            // Assert 
            Assert.AreEqual(item, result);
        }

        [TestMethod]
        public void HasWeapon_ReturnTrue()
        {
            // Arrange 
            var inventory = new Inventory();
            inventory.AddItem(new Item("Sword", "A sharp blade.", ItemType.Weapon));

            // Act 
            bool result = inventory.HasWeapon();

            // Assert
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void GetStrongestWeapon_ReturnWeaponWithHighestDamage()
        {
            // Arrange 
            var inventory = new Inventory();
            var sword = new Item("Sword", "Basic sword", ItemType.Weapon, 10);
            var lance = new Item("Lance", "Sharp lance", ItemType.Weapon, 20);
            inventory.AddItem(sword);
            inventory.AddItem(lance);

            // Act 
            var result = inventory.GetBestWeapon();

            // Assert
            Assert.AreEqual(lance, result);
        }
        [TestMethod]
        public void GetAllItems_returnsCopyOfList()
        {
            // Arrange
            var inventory = new Inventory();
            var item = new Item("Key", "Opens a door.");
            inventory.AddItem(item);

            // Act
            var list = inventory.GetAllItems();
            list.Clear();

            // Assert
            Assert.AreEqual(1, inventory.GetItemCount());
        }

        [TestMethod]
        public void MaxCapacity_NotAddItem()
        {
            // Arragne 
            var inventory = new Inventory { MaxCapacity = 2 };
            inventory.AddItem(new Item("Daggers", "Sharp little daggers."));
            inventory.AddItem(new Item("Knife", "Good old knife."));

            // Act 
            inventory.AddItem(new Item("Sword", "desc"));

            // Assert
            Assert.AreEqual(2, inventory.GetItemCount());
        }
    }
}
