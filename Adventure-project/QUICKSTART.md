# Quick Start Guide

## Step 1: Create Encrypted Room Files (One-time setup)

Open a terminal/command prompt and run:

```bash
# Navigate to EncryptionTool
cd Adventure-project/EncryptionTool

# Build the encryption tool
dotnet build

# Encrypt the secret room (from Adventure-project directory)
dotnet run -- ../room_secret.txt ../room_secret.enc secret mypassphrase123

# Encrypt the treasure room
dotnet run -- ../room_treasure.txt ../room_treasure.enc treasure treasurepass456
```

This creates the encrypted `.enc` files needed for the game.

## Step 2: Start the API Server

Open a **new terminal/command prompt** and run:

```bash
# Navigate to API directory
cd Adventure-project/AdventureAPI

# Restore packages (first time only)
dotnet restore

# Run the API
dotnet run
```

The API will start on `http://localhost:5196`
- Swagger UI: http://localhost:5196/swagger

**Keep this terminal open!** The API must be running for the game to work.

## Step 3: Run the Game Client

Open **another new terminal/command prompt** and run:

```bash
# Navigate to game directory
cd Adventure-project

# Restore packages (first time only)
dotnet restore

# Run the game
dotnet run
```

## Step 4: Play the Game!

1. **Register a new user:**
   - Choose option `2` (Register)
   - Enter username (min 3 characters)
   - Enter password (min 6 characters)
   - Enter role: `Player` or `Admin` (default: Player)

2. **Login:**
   - Choose option `1` (Login)
   - Enter your credentials

3. **Game Commands:**
   - `north`, `south`, `east`, `west`, `up`, `down` - Move
   - `look` - Examine room
   - `take <item>` - Pick up items
   - `inventory` - View inventory
   - `fight` - Fight monsters
   - `decrypt <room>` - Decrypt encrypted rooms (requires Admin role + passphrase)
   - `noclip <room>` - Teleport to any room (Admin only)
   - `help` - Show all commands

## Testing Encrypted Rooms

1. **As Admin user:**
   - Navigate to an encrypted room (Secret or Treasure)
   - Use `decrypt secret` or `decrypt treasure`
   - Enter passphrase when prompted:
     - Secret room: `mypassphrase123`
     - Treasure room: `treasurepass456`

2. **As Player user:**
   - You can enter encrypted rooms but cannot decrypt them (no keyshare access)

## Troubleshooting

- **"API connection failed"**: Make sure the API is running (Step 2)
- **"Decryption failed"**: 
  - Verify you're logged in as Admin
  - Check that `.enc` files exist in the Adventure-project directory
  - Verify the passphrase is correct
- **Build errors**: Run `dotnet restore` in each project directory

## Notes

- The API stores users in-memory (users are lost when API restarts)
- JWT tokens expire after 24 hours
- Account locks for 15 minutes after 3 failed login attempts
- Encrypted files must be in the same directory as the game executable

