
using Adventure_project;
using System.Net.Http.Headers;

namespace TextAdventure_tests

{
    [TestClass]
    public sealed class ItemTest
    {
        [TestMethod]
        public void Set_correct_constructor_values()
        {
            // Arrange 
            string name = "Sword";
            string description = "A sharp blade.";
            ItemType type = ItemType.Weapon;
            int value = 10;

            // Act
            var item = new Item(name, description, type, value);

            // Assert 
            Assert.AreEqual(name, item.Name);
            Assert.AreEqual(description, item.Description);
            Assert.AreEqual(type, item.Type);
            Assert.AreEqual(value, item.Value);



        }

        [TestMethod]
        public void IsWeapon_returnTrue()
        {
            // Arrange
            var item = new Item("Sword", "A sharp blade.", ItemType.Weapon);

            // Act
            bool result = item.IsWeapon();

            // Assert 
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsKey_returnTrue()
        {
            // Arrange 
            var item = new Item("Key", "Opens a door.", ItemType.Key);

            // Act 
            bool result = item.IsKey();

            // Assert 
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void WhenTypeNotWeapon_returnFalse()
        {
            // Arrange 
            var item = new Item("Key", "Opens a door.", ItemType.Key);

            // Act 
            bool result = item.IsWeapon();

            // Assert 
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void WhenTypeNotKey_returnFalse()
        {
            // Arrange
            var item = new Item("Sword", "A sharp blade", ItemType.Weapon);

            // Act 
            bool result = item.IsKey();

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void ToString_returnName_description()
        {
            // Arrange 
            var item = new Item("Potion", "Restores health.");

            // Act 
            string result = item.ToString();

            // Assert
           Assert.AreEqual("Potion - Restores health.", result);
        }

        [TestMethod]
        public void WhenNoTypeOrValueGiven_setDefaultValues()
        {
            // Arrange 
            string name = "Stone";
            string description = "A simple rock.";

            // Act 
            var item = new Item(name, description);

            // Assert
            Assert.AreEqual(ItemType.Generic, item.Type);
            Assert.AreEqual(0, item.Value);
        }

        [TestMethod]
        public void ValuesUpdate_withSetters()
        {
            // Arrange 
            var item = new Item("Old Sword", "Rusty weapon");

            // Act 
            item.Name = "New Sword";
            item.Description = "Shiny and sharp";
            item.Type = ItemType.Weapon;
            item.Value = 20;

            // Assert
            Assert.AreEqual("New Sword", item.Name);
            Assert.AreEqual("Shiny and sharp", item.Description);
            Assert.AreEqual(ItemType.Weapon, item.Type);
            Assert.AreEqual(20, item.Value);
        }
        [TestMethod]
        public void EmptyValues_toString()
        {
            // Arrange 
            var item = new Item("", "");

            // Act 
            string result = item.ToString();

            // Assert 
            Assert.AreEqual(" - ", result);
        }
    }


    //public sealed class IntegrationTest
    //{
    //    [TestMethod]
    //    public void TestMethod2()
    //    {

    //    }
    //}

    //public sealed class BehaviorDrivenTest
    //{
    //    [TestMethod]
    //    public void TestMethod3()
    //    {

    //    }
    //}
}
