# Secure Text Adventure 2.0

A secure text adventure game with API authentication, encryption, and secure coding practices.

## Project Structure

```
Adventure-project/
├── Adventure-project/          # Main game client
│   ├── Program.cs              # Main game loop with authentication
│   ├── Services/               # API client and encryption services
│   ├── Room.cs                 # Room class with encryption support
│   ├── Rooms.cs                # Game world management
│   └── GameSetup.cs            # World setup with encrypted rooms
├── AdventureAPI/               # Authentication API server
│   ├── Program.cs              # API endpoints
│   ├── Services/               # Auth and keyshare services
│   └── Models/                 # Data models
├── EncryptionTool/            # Tool to encrypt room files
└── room_*.enc                 # Encrypted room files
```

## Features

### Security Features
- ✅ API-based user authentication
- ✅ SHA-256 password hashing
- ✅ JWT token authentication
- ✅ Account lockout after 3 failed login attempts
- ✅ Encrypted rooms using CMS encryption
- ✅ Keyshare-based decryption
- ✅ Role-based access control (Player/Admin)
- ✅ Secure input handling
- ✅ Error handling without exposing internals

### Game Features
- Room navigation
- Item collection
- Monster combat
- Encrypted rooms (requires decryption)
- Admin noclip command
- Inventory management

## Prerequisites

- .NET 8.0 SDK
- Two terminal windows (one for API, one for client)

## Setup Instructions

### 1. Start the API Server

```bash
cd Adventure-project/AdventureAPI
dotnet restore
dotnet run
```

The API will start on `http://localhost:5196`

### 2. Create Encrypted Room Files

First, create the encryption tool:
```bash
cd Adventure-project/EncryptionTool
dotnet restore
dotnet build
```

Then encrypt the room files:
```bash
# Encrypt secret room
dotnet run -- room_secret.txt room_secret.enc secret mypassphrase123

# Encrypt treasure room
dotnet run -- room_treasure.txt room_treasure.enc treasure treasurepass456
```

**Important**: Copy the encrypted `.enc` files to the `Adventure-project` directory (where the game executable runs).

### 3. Run the Game Client

In a new terminal:
```bash
cd Adventure-project
dotnet restore
dotnet run
```

## Usage

### First Time Setup

1. **Register a User**:
   - Choose option 2 (Register)
   - Enter username (min 3 characters)
   - Enter password (min 6 characters)
   - Enter role: "Player" or "Admin" (default: Player)

2. **Login**:
   - Choose option 1 (Login)
   - Enter your credentials

### Game Commands

**Movement:**
- `north` / `n` - Move north
- `south` / `s` - Move south
- `east` / `e` - Move east
- `west` / `w` - Move west
- `up` / `u` - Move up
- `down` / `d` - Move down

**Actions:**
- `look` / `l` - Examine current room
- `take <item>` - Pick up an item
- `inventory` / `inv` / `i` - View inventory
- `fight` / `attack` - Fight a monster
- `status` - Show player status
- `decrypt <room>` - Decrypt an encrypted room (requires keyshare + passphrase)

**Admin Commands:**
- `noclip <room>` - Teleport to any room (Admin only)

**Other:**
- `help` / `h` / `?` - Show help
- `quit` / `exit` / `q` - Quit game

### Encrypted Rooms

Some rooms are encrypted and require decryption:

1. Enter an encrypted room (you'll see a lock icon)
2. Use `decrypt <room_name>` command
3. Enter the passphrase when prompted
4. The game will:
   - Fetch the keyshare from the API (Admin role required)
   - Generate decryption key: `SHA256(keyshare + ":" + passphrase)`
   - Decrypt and display the room contents

**Example:**
```
> decrypt secret
Enter passphrase: ********
[Room content displayed]
```

### Admin Features

Users with "Admin" role can:
- Access keyshares via API
- Use `noclip` command to teleport to any room
- Note: Even Admins cannot read encrypted room contents without proper decryption

## Security Implementation Details

### Password Hashing
- Passwords are hashed using SHA-256
- Hashes are stored (not plaintext passwords)
- Login compares hash of input with stored hash

### JWT Tokens
- Tokens contain username and role claims
- Tokens expire after 24 hours
- Tokens are sent in Authorization header

### Account Lockout
- After 3 failed login attempts, account is locked for 15 minutes
- Lockout counter resets on successful login

### Encryption
- Room files are encrypted using AES-256-CBC
- Decryption key: `SHA256(keyshare + ":" + passphrase)`
- Keyshares are stored on the API (Admin access only)
- Passphrases are user-provided

### Secure Coding
- Input sanitization
- Error handling without exposing internals
- No secrets in code
- Proper exception handling

## Configuration

### API URL
Set environment variable to change API URL:
```bash
# Windows PowerShell
$env:API_URL="http://localhost:5196"

# Linux/Mac
export API_URL="http://localhost:5196"
```

### JWT Settings
Edit `AdventureAPI/appsettings.json`:
```json
{
  "Jwt": {
    "Key": "YourSecretKeyHere",
    "Issuer": "AdventureAPI",
    "Audience": "AdventureClient"
  }
}
```

## Troubleshooting

### API Connection Failed
- Ensure API is running on the correct port
- Check firewall settings
- Verify API_URL environment variable

### Decryption Failed
- Verify you have Admin role
- Check that keyshare matches room ID
- Ensure passphrase is correct
- Confirm encrypted file exists in game directory

### Login Issues
- Check username/password are correct
- Wait if account is locked (15 minutes)
- Verify API is running

## Development Notes

- User data is stored in-memory (not persisted)
- Encrypted files must be in the same directory as the game executable
- API must be running before starting the game
- Keyshares are hardcoded in `KeyshareService` (in production, use a database)

## License

This project is for educational purposes.

