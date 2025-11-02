using Adventure_project;
using System;
using System.IO;
using System.Linq;

namespace TextAdventure_tests
{
    [TestClass]
    public sealed class IntegrationTests
    {
        private TextWriter? _originalOutput;

        [TestInitialize]
        public void Setup()
        {
            // Redirect console output to a null writer to suppress noise during tests
            _originalOutput = Console.Out;
            Console.SetOut(TextWriter.Null);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Restore console output
            if (_originalOutput != null)
            {
                Console.SetOut(_originalOutput);
            }
        }

        [TestMethod]
        public void Rooms_Inventory_Integration_TakingItemFromRoom()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Item sword = new Item("Sword", "A sharp blade", ItemType.Weapon, 15);
            
            startRoom.AddItem(sword);
            world.AddRoom(startRoom);
            world.SetSpawnRoom("Start");

            // Act
            world.TakeItem("Sword");

            // Assert
            Assert.AreEqual(1, world.PlayerInv.GetItemCount(), "Item should be in inventory");
            Assert.IsTrue(world.PlayerInv.HasItem("Sword"), "Inventory should contain the sword");
            Assert.AreEqual(0, startRoom.Items.Count, "Room should no longer have the item");
        }

        [TestMethod]
        public void Rooms_Room_Movement_Integration_BasicNavigation()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room northRoom = new Room("North", "North room");
            
            startRoom.AddExit(Directions.North, northRoom);
            northRoom.AddExit(Directions.South, startRoom);
            
            world.AddRoom(startRoom);
            world.AddRoom(northRoom);
            world.SetSpawnRoom("Start");

            // Act
            world.Move(Directions.North);

            // Assert
            Assert.AreEqual(northRoom.Name, world.CurrentRoom?.Name, "Player should be in north room");
            Assert.IsFalse(world.GameOver, "Game should not be over");
        }

        [TestMethod]
        public void Rooms_Room_Inventory_KeyIntegration_UnlockingDoor()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room lockedRoom = new Room("Locked", "Locked room");
            lockedRoom.RequiresKey = true;
            
            Item key = new Item("Key", "Golden key", ItemType.Key, 1);
            startRoom.AddItem(key);
            
            startRoom.AddExit(Directions.North, lockedRoom);
            lockedRoom.AddExit(Directions.South, startRoom);
            
            world.AddRoom(startRoom);
            world.AddRoom(lockedRoom);
            world.SetSpawnRoom("Start");

            // Act - Take key first
            world.TakeItem("Key");
            Assert.IsTrue(world.PlayerInv.HasKey(), "Player should have the key");

            // Try to move into locked room
            world.Move(Directions.North);

            // Assert
            Assert.AreEqual(lockedRoom.Name, world.CurrentRoom?.Name, "Player should enter the locked room");
            Assert.IsFalse(lockedRoom.RequiresKey, "Door should be unlocked after using key");
        }

        [TestMethod]
        public void Rooms_Room_KeyIntegration_CannotEnterWithoutKey()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room lockedRoom = new Room("Locked", "Locked room");
            lockedRoom.RequiresKey = true;
            
            startRoom.AddExit(Directions.North, lockedRoom);
            lockedRoom.AddExit(Directions.South, startRoom);
            
            world.AddRoom(startRoom);
            world.AddRoom(lockedRoom);
            world.SetSpawnRoom("Start");

            // Act - Try to move without key
            world.Move(Directions.North);

            // Assert
            Assert.AreEqual(startRoom.Name, world.CurrentRoom?.Name, "Player should remain in start room");
            Assert.IsTrue(lockedRoom.RequiresKey, "Door should still be locked");
        }

        [TestMethod]
        public void Rooms_Room_Monster_Integration_MonsterBlocksMovement()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room monsterRoom = new Room("Monster", "Room with monster");
            
            Monster orc = Monster.CreateOrc();
            world.AddRoom(startRoom);
            world.AddRoom(monsterRoom);
            world.AddMonsterToRoom("Monster", orc);
            
            startRoom.AddExit(Directions.North, monsterRoom);
            monsterRoom.AddExit(Directions.South, startRoom);

            world.SetSpawnRoom("Start");

            // Act - Try to move into room with alive monster
            world.Move(Directions.North);

            // Assert
            Assert.AreEqual(startRoom.Name, world.CurrentRoom?.Name, "Player should be blocked by monster");
            Assert.IsTrue(monsterRoom.HasMonster, "Room should have monster");
            Assert.IsTrue(monsterRoom.IsMonsterAlive, "Monster should still be alive");
        }

        [TestMethod]
        public void Rooms_Inventory_Monster_CombatIntegration_FightAndDefeatMonster()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room monsterRoom = new Room("Monster", "Room with monster");
            
            Item sword = new Item("Sword", "Sharp blade", ItemType.Weapon, 100); // High damage to kill quickly
            Monster weakOrc = new Monster("Weak Orc", "Weak enemy", 10, 1, 0); // Low health, low damage
            
            startRoom.AddItem(sword);
            world.AddRoom(startRoom);
            world.AddRoom(monsterRoom);
            world.AddMonsterToRoom("Monster", weakOrc);
            
            startRoom.AddExit(Directions.North, monsterRoom);
            monsterRoom.AddExit(Directions.South, startRoom);

            world.SetSpawnRoom("Start");
            int initialHealth = world.PlayerHealth;

            // Act - Take weapon
            world.TakeItem("Sword");
            
            // Move to monster room (should be blocked initially)
            world.Move(Directions.North);
            Assert.AreEqual(startRoom.Name, world.CurrentRoom?.Name, "Should be blocked");

            // Simulate combat by manually attacking monster (since FightMonster requires console input)
            // We'll simulate the combat logic
            string roomKey = monsterRoom.Name.ToLower();
            Monster monster = world.RoomMonsters[roomKey];
            
            // Player attacks
            Item weapon = world.PlayerInv.GetBestWeapon();
            monster.TakeDamage(weapon.Value);
            
            // If monster is dead, update room state
            if (!monster.IsAlive)
            {
                monsterRoom.IsMonsterAlive = false;
            }

            // Now try to move again
            world.Move(Directions.North);

            // Assert
            Assert.AreEqual(monsterRoom.Name, world.CurrentRoom?.Name, "Player should enter room after monster defeat");
            Assert.IsFalse(monster.IsAlive, "Monster should be defeated");
            Assert.IsFalse(monsterRoom.IsMonsterAlive, "Room should reflect monster is dead");
        }

        [TestMethod]
        public void Rooms_Room_DeadlyRoom_Integration_TakesDamage()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room deadlyRoom = new Room("Deadly", "Poisonous room");
            deadlyRoom.IsDeadly = true;
            
            startRoom.AddExit(Directions.North, deadlyRoom);
            deadlyRoom.AddExit(Directions.South, startRoom);
            
            world.AddRoom(startRoom);
            world.AddRoom(deadlyRoom);
            world.SetSpawnRoom("Start");

            int initialHealth = world.PlayerHealth;

            // Act
            world.Move(Directions.North);

            // Assert
            Assert.AreEqual(50, initialHealth - world.PlayerHealth, "Player should take 50 damage");
            Assert.AreEqual(deadlyRoom.Name, world.CurrentRoom?.Name, "Player should be in deadly room");
        }

        [TestMethod]
        public void Rooms_Room_DeadlyRoom_Integration_PlayerDies()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room deadlyRoom = new Room("Deadly", "Poisonous room");
            deadlyRoom.IsDeadly = true;
            
            startRoom.AddExit(Directions.North, deadlyRoom);
            deadlyRoom.AddExit(Directions.South, startRoom);
            
            world.AddRoom(startRoom);
            world.AddRoom(deadlyRoom);
            world.SetSpawnRoom("Start");

            // Set player health low enough to die from deadly room (50 damage)
            world.PlayerHealth = 50;

            // Act
            world.Move(Directions.North);

            // Assert
            Assert.IsTrue(world.GameOver, "Game should be over when player dies");
            Assert.AreEqual(0, world.PlayerHealth, "Player health should be 0");
        }

        [TestMethod]
        public void Monster_DropLoot_Room_Integration_LootAppearsInRoom()
        {
            // Arrange
            Rooms world = new Rooms();
            Room monsterRoom = new Room("Monster", "Room with monster");
            
            Monster orc = Monster.CreateOrc();
            Item potion = new Item("Potion", "Heals you", ItemType.Consumable, 20);
            orc.AddLoot(potion);
            
            world.AddRoom(monsterRoom);
            world.AddMonsterToRoom("Monster", orc);

            // Act - Defeat monster and drop loot
            orc.TakeDamage(100); // Kill the monster
            if (!orc.IsAlive)
            {
                List<Item> loot = orc.DropLoot();
                foreach (Item item in loot)
                {
                    monsterRoom.AddItem(item);
                }
            }

            // Assert
            Assert.AreEqual(1, monsterRoom.Items.Count, "Room should have dropped loot");
            Assert.IsTrue(monsterRoom.Items.Any(i => i.Name == "Potion"), "Room should contain the potion");
        }

        [TestMethod]
        public void Rooms_GameSetup_Integration_CompleteWorldSetup()
        {
            // Arrange
            GameSetup setup = new GameSetup();

            // Act
            Rooms world = setup.CreateWorld();

            // Assert
            Assert.IsNotNull(world, "World should be created");
            Assert.IsNotNull(world.CurrentRoom, "Current room should be set");
            Assert.AreEqual("Start", world.CurrentRoom.Name, "Should start in Start room");
            Assert.IsNotNull(world.PlayerInv, "Player should have inventory");
            Assert.AreEqual(100, world.PlayerHealth, "Player should start with 100 health");
            Assert.IsTrue(world.AllRooms.Count > 0, "World should have rooms");
        }

        [TestMethod]
        public void FullGameFlow_Integration_PickWeapon_FightMonster_GetLoot()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room monsterRoom = new Room("Monster", "Room with monster");
            
            Item sword = new Item("Sword", "Sharp blade", ItemType.Weapon, 60);
            Monster weakOrc = new Monster("Weak Orc", "Weak enemy", 50, 1, 0);
            Item potion = new Item("Potion", "Heals", ItemType.Consumable, 20);
            weakOrc.AddLoot(potion);
            
            startRoom.AddItem(sword);
            world.AddRoom(startRoom);
            world.AddRoom(monsterRoom);
            world.AddMonsterToRoom("Monster", weakOrc);
            
            startRoom.AddExit(Directions.North, monsterRoom);
            monsterRoom.AddExit(Directions.South, startRoom);

            world.SetSpawnRoom("Start");

            // Act - Step 1: Pick up weapon
            world.TakeItem("Sword");
            Assert.IsTrue(world.PlayerInv.HasWeapon(), "Should have weapon");

            // Step 2: Move to monster (blocked)
            world.Move(Directions.North);
            Assert.AreEqual(startRoom.Name, world.CurrentRoom?.Name, "Blocked by monster");

            // Step 3: Simulate combat - attack until monster dies
            string roomKey = monsterRoom.Name.ToLower();
            Monster monster = world.RoomMonsters[roomKey];
            Item weapon = world.PlayerInv.GetBestWeapon();
            
            while (monster.IsAlive)
            {
                monster.TakeDamage(weapon.Value);
            }
            
            // Update room state after combat
            if (!monster.IsAlive)
            {
                monsterRoom.IsMonsterAlive = false;
                List<Item> loot = monster.DropLoot();
                foreach (Item item in loot)
                {
                    monsterRoom.AddItem(item);
                }
            }

            // Step 4: Move into room
            world.Move(Directions.North);
            Assert.AreEqual(monsterRoom.Name, world.CurrentRoom?.Name, "Should enter room");

            // Step 5: Collect loot
            int initialInventoryCount = world.PlayerInv.GetItemCount();
            world.TakeItem("Potion");

            // Assert - Verify complete flow
            Assert.IsTrue(world.PlayerInv.HasItem("Sword"), "Should have sword");
            Assert.IsTrue(world.PlayerInv.HasItem("Potion"), "Should have collected potion");
            Assert.AreEqual(initialInventoryCount + 1, world.PlayerInv.GetItemCount(), "Inventory should increase");
            Assert.IsFalse(monster.IsAlive, "Monster should be defeated");
        }

        [TestMethod]
        public void Rooms_Inventory_WeaponSelection_Integration_GetBestWeapon()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            
            Item dagger = new Item("Dagger", "Small blade", ItemType.Weapon, 5);
            Item sword = new Item("Sword", "Big blade", ItemType.Weapon, 15);
            Item axe = new Item("Axe", "Heavy weapon", ItemType.Weapon, 25);
            
            startRoom.AddItem(dagger);
            startRoom.AddItem(sword);
            startRoom.AddItem(axe);
            
            world.AddRoom(startRoom);
            world.SetSpawnRoom("Start");

            // Act - Collect all weapons
            world.TakeItem("Dagger");
            world.TakeItem("Sword");
            world.TakeItem("Axe");

            // Assert
            Item bestWeapon = world.PlayerInv.GetBestWeapon();
            Assert.AreEqual("Axe", bestWeapon.Name, "Should get the strongest weapon");
            Assert.AreEqual(25, bestWeapon.Value, "Best weapon should have highest damage");
        }

        [TestMethod]
        public void Rooms_WinCondition_Integration_ReachingWinRoom()
        {
            // Arrange
            Rooms world = new Rooms();
            Room startRoom = new Room("Start", "Starting room");
            Room winRoom = new Room("Win", "Victory room");
            
            startRoom.AddExit(Directions.North, winRoom);
            winRoom.AddExit(Directions.South, startRoom);
            
            world.AddRoom(startRoom);
            world.AddRoom(winRoom);
            world.SetSpawnRoom("Start");

            // Act
            world.Move(Directions.North);

            // Assert
            Assert.IsTrue(world.GameWon, "Game should be won");
            Assert.IsTrue(world.GameOver, "Game should be over");
            Assert.AreEqual(winRoom.Name, world.CurrentRoom?.Name, "Should be in win room");
        }
    }
}

