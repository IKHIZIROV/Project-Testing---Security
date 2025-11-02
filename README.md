# 🏰 Castle Adventure Game

A text-based adventure game built in C# .NET 8.0 with comprehensive testing using Unit Tests, Integration Tests, and Behavior-Driven Development (BDD).

## 📋 Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Testing Approach](#testing-approach)
- [How to Play](#how-to-play)
- [Running Tests](#running-tests)
- [Requirements](#requirements)

## 🎮 Overview

Castle Adventure is an interactive text-based game where players explore a castle, collect items, fight monsters, and solve puzzles to reach the throne room. The game features:

- **Room Navigation**: Move between connected rooms in 6 directions (North, South, East, West, Up, Down)
- **Item Management**: Collect weapons, keys, and consumables
- **Combat System**: Fight monsters using weapons from your inventory
- **Puzzle Elements**: Unlock doors with keys, avoid deadly rooms
- **Win Condition**: Reach the throne room to win the game

## 📁 Project Structure

```
Adventure-project/
├── Adventure-project/          # Main game project
│   ├── Program.cs              # Game entry point and main loop
│   ├── GameSetup.cs            # World initialization
│   ├── Rooms.cs                # Game world controller
│   ├── Room.cs                 # Individual room implementation
│   ├── Inventory.cs            # Player inventory management
│   ├── Item.cs                 # Item definitions and types
│   ├── Monster.cs              # Monster/enemy definitions
│   └── Adventure-project.csproj
│
└── TextAdventure_tests/        # Test project
    ├── Unit Tests/
    │   ├── InventoryTests.cs
    │   ├── ItemTests.cs
    │   ├── MonsterTests.cs
    │   └── RoomTests.cs
    │
    ├── Integration Tests/
    │   └── IntegrationTests.cs
    │
    ├── BDD Tests (SpecFlow)/
    │   ├── Features/
    │   │   ├── Gameplay.feature
    │   │   ├── Combat.feature
    │   │   └── CompleteGameFlow.feature
    │   └── StepDefinitions/
    │       └── GameplayStepDefinitions.cs
    │
    └── TextAdventure_tests.csproj
```

## 🧪 Testing Approach

This project follows a comprehensive three-layer testing strategy:

### 1. **Unit Tests** (MSTest)

Tests individual classes in isolation to verify core functionality:

- **InventoryTests**: Item management, capacity limits, weapon selection
- **ItemTests**: Item properties, type checking, value validation
- **MonsterTests**: Combat mechanics, damage calculation, loot drops
- **RoomTests**: Room initialization, item handling, exits, monster management

**Total**: ~51 unit tests

### 2. **Integration Tests** (MSTest)

Tests interactions between multiple classes to ensure they work together:

- Room and Inventory integration
- Movement and navigation
- Key-based door unlocking
- Monster blocking mechanics
- Combat system integration
- Deadly room damage
- Complete gameplay flows

**Total**: 13 integration tests

### 3. **Behavior-Driven Development (BDD)** (SpecFlow/Gherkin)

Uses Gherkin syntax to describe game scenarios in plain English:

- **Gameplay.feature**: Winning, losing, item collection, door unlocking
- **Combat.feature**: Monster combat, loot drops, weapon requirements
- **CompleteGameFlow.feature**: End-to-end game scenarios

**Total**: 15 BDD scenarios

### Testing Statistics

- **Unit Tests**: 51+ tests
- **Integration Tests**: 13 tests
- **BDD Scenarios**: 15 scenarios (generates multiple test methods)
- **Total**: 80+ test scenarios

## 🎯 How to Play

### Getting Started

1. **Build the project**:

   ```bash
   dotnet build Adventure-project/Adventure-project.csproj
   ```

2. **Run the game**:
   ```bash
   dotnet run --project Adventure-project/Adventure-project.csproj
   ```

### Gameplay

#### Objective

Navigate through the castle, collect items, defeat monsters, and reach the **Win** room (throne room).

#### Commands

**Movement:**

- `north` / `n` - Move north
- `south` / `s` - Move south
- `east` / `e` - Move east
- `west` / `w` - Move west
- `up` / `u` - Move up
- `down` / `d` - Move down

**Actions:**

- `look` / `l` - Examine current room
- `take [item]` - Pick up an item (e.g., `take sword`)
- `inventory` / `inv` / `i` - View your inventory
- `fight` / `attack` - Fight a monster in the room
- `status` / `stats` - Show health and current status

**Other:**

- `help` / `h` / `?` - Show help menu
- `quit` / `exit` / `q` - Quit the game

### Game Mechanics

1. **Items**: Collect weapons (for combat), keys (to unlock doors), and consumables
2. **Combat**: You need a weapon to fight monsters. Combat is turn-based
3. **Keys**: Some doors are locked and require a key to enter
4. **Deadly Rooms**: Some rooms deal 50 damage when entered - be careful!
5. **Health**: Starting health is 100. If it reaches 0, you die
6. **Monsters**: Living monsters block room entrances until defeated

### Example Playthrough

```
> look                    # See what's in the room
> take sword              # Pick up the sword
> east                    # Move to the east room
> take key                # Get the key
> north                   # Move to locked room (auto-unlocks with key)
> down                    # Go to monster room
> fight                   # Fight the monster
> north                   # Move to win room - Victory!
```

## 🧪 Running Tests

### Run All Tests

```bash
dotnet test TextAdventure_tests/TextAdventure_tests.csproj
```

### Run Specific Test Categories

**Unit Tests Only:**

```bash
dotnet test --filter "FullyQualifiedName~Tests&!FullyQualifiedName~IntegrationTests&!FullyQualifiedName~Gameplay&!FullyQualifiedName~Combat&!FullyQualifiedName~CompleteGameFlow"
```

**Integration Tests:**

```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

**BDD Tests (SpecFlow):**

```bash
dotnet test --filter "FullyQualifiedName~Gameplay|FullyQualifiedName~Combat|FullyQualifiedName~CompleteGameFlow"
```

**Specific Test Class:**

```bash
dotnet test --filter "FullyQualifiedName~RoomTests"
```

### Test Results

All tests should pass. The test suite validates:

- ✅ Individual class functionality (unit tests)
- ✅ Class interactions (integration tests)
- ✅ Game behavior scenarios (BDD tests)

## 📦 Requirements

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** or **Visual Studio Code** (optional)
- **Windows/Linux/macOS** (cross-platform)

### NuGet Packages (Auto-installed)

- MSTest Framework 3.6.4
- SpecFlow 3.9.74 (for BDD testing)
- Microsoft.NET.Test.Sdk 17.12.0

## 🏗️ Architecture

### Core Classes

- **`Rooms`**: Main game controller managing world state, player movement, combat, and inventory
- **`Room`**: Individual rooms with items, monsters, exits, and special properties
- **`Inventory`**: Player item storage with capacity limits and query methods
- **`Item`**: Items with types (Weapon, Key, Consumable, Treasure) and properties
- **`Monster`**: Enemies with health, attack power, defense, and loot tables
- **`GameSetup`**: Creates and initializes the game world

### Design Patterns

- **Factory Pattern**: `Monster.CreateOrc()`, `Monster.CreateGoblin()`, etc.
- **Composition**: Rooms contain Items and Monsters
- **State Management**: Game state tracked in Rooms class

**Enjoy your adventure! 🗡️🛡️👑**
