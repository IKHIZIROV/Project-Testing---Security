
using Adventure_project;
using System.Net.Http.Headers;

namespace TextAdventure_tests

{
    [TestClass]
    public sealed class UnitTest
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
        public void ToString_returnName_Description()
        {
            // Arrange 
            var item = new Item("Potion", "Restores health.");

            // Act 
            string result = item.ToString();

            // Assert
           Assert.AreEqual("Potion - Restores health.", result);
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
