using Adventure_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_tests
{
    [TestClass]
    public sealed class  MonsterTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeAllProperties()
        {
            // Arrange & Act
            var monster = new Monster("Goblin", "Small creature", 30, 8, 2, MonsterType.Goblinoid);

            // Assert
            Assert.AreEqual("Goblin", monster.Name);
            Assert.AreEqual("Small creature", monster.Description);
            Assert.AreEqual(30, monster.Health);
            Assert.AreEqual(8, monster.AttackPower);
            Assert.AreEqual(2, monster.DefenseRating);
            Assert.AreEqual(MonsterType.Goblinoid, monster.Type);
            Assert.IsTrue(monster.IsAlive);
            Assert.AreEqual(0, monster.LootTable.Count);
        }
        [TestMethod]
        public void TakeDamage_ShouldReduceHealth()
        {
            // Arrange 
            var monster = new Monster("Orc", "Strong enemy", 50, 10, defenseRating: 5);

            // Act 
            monster.TakeDamage(10);

            // Assert uitkomst is 10 - 5 = 5 schade
            Assert.AreEqual(45, monster.Health);
            Assert.IsTrue(monster.IsAlive);
        }
        [TestMethod]
        public void TakeDamage_ShouldKillMonster_WhenHealthIsZero()
        {
            // Arrange 
            var monster = new Monster("Goblin", "Weak", 5, 10, 0);

            // Act 
            monster.TakeDamage(10);

            // Assert
            Assert.AreEqual(0, monster.Health);
            Assert.IsFalse(monster.IsAlive);
        }
        [TestMethod]
        public void TakeDamage_ShouldAlwaysDealthAtLeastOneDamage()
        {
            // Arrange 
            var monster = new Monster("Dragon", "Very strong", 100, 20, defenseRating: 100);

            // Act 
            monster.TakeDamage(10);

            // Assert
            Assert.AreEqual(99, monster.Health);
        }
        [TestMethod]
        public void AddLoot_ShouldAddItemToLootTable()
        {
            // Arrange 
            var monster = new Monster("Goblin", "Loot test", 30, 8);
            var item = new Item("Gold", "A shiny coin", ItemType.Treasure, 10);

            // Act 
            monster.AddLoot(item);

            // Assert
            Assert.AreEqual(1, monster.LootTable.Count);
            Assert.AreEqual(item, monster.LootTable[0]);
        }
        [TestMethod]
        public void DropLoot_ShouldREturnLoot_WhenMonsterIsDead()
        {
            // Arrange 
            var monster = new Monster("Orc", "Dead orc", 0, 10);
            var potion = new Item("Potion", "Healing item", ItemType.Consumable);
            monster.AddLoot(potion);
            monster.IsAlive = false;

            // Act 
            var loot = monster.DropLoot();

            // Assert
            Assert.AreEqual(1, loot.Count);
            Assert.AreEqual(potion, loot[0]);
        }
        [TestMethod]
        public void DropLoot_ShouldReturnEmptyList_WhenMonsterIsAlive()
        {
            // Arrange 
            var monster = new Monster("Goblin", "Alive goblin", 10, 5);
            monster.AddLoot(new Item("Coin", "Shiny gold", ItemType.Treasure));

            // Act 
            var loot = monster.DropLoot();

            // Assert 
            Assert.AreEqual(0, loot.Count);
        }
        [TestMethod]
        public void CreateGoblin_ShouldReturnGoblinType()
        {
            // Act 
            var goblin = Monster.CreateGoblin();

            // Assert
            Assert.AreEqual("Goblin", goblin.Name);
            Assert.AreEqual(MonsterType.Goblinoid, goblin.Type);
            Assert.IsTrue(goblin.IsAlive);
        }
        [TestMethod]
        public void CreateDragon_ShouldHaveLoot()
        {
            // Act 
            var dragon = Monster.CreateDragon();

            // Assert
            Assert.AreEqual("Dragon", dragon.Name);
            Assert.IsTrue(dragon.LootTable.Count > 0);
        }
        [TestMethod]
        public void Attack_ShouldReturnZero_WhenMonsterIsDead()
        {
            // Arrange 
            var monster = new Monster("Skeleton", "Dead", 0, 10);
            monster.IsAlive = false;

            // Act
            var damage = monster.Attack();

            // Assert
            Assert.AreEqual(0, damage);
        }
    }
}