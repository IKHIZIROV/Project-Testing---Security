# Adventure API - Secure Authentication Service

This is a Minimal API project that provides secure authentication and keyshare management for the Text Adventure game.

## Features

- **User Registration**: Register new users with username, password, and role (Player/Admin)
- **User Login**: Secure login with SHA-256 password hashing
- **JWT Authentication**: Token-based authentication for secure API access
- **Account Lockout**: Automatic lockout after 3 failed login attempts (15 minutes)
- **Keyshare Management**: Admin-only access to keyshares for encrypted rooms
- **Role-Based Access**: Support for Player and Admin roles

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code (optional)

## Setup

1. Navigate to the AdventureAPI directory:
   ```bash
   cd Adventure-project/AdventureAPI
   ```

2. Restore packages:
   ```bash
   dotnet restore
   ```

3. Update `appsettings.json` with your JWT configuration (optional):
   ```json
   {
     "Jwt": {
       "Key": "YourSuperSecretKeyForJWTTokenGenerationThatIsAtLeast32CharactersLong!",
       "Issuer": "AdventureAPI",
       "Audience": "AdventureClient"
     }
   }
   ```

## Running the API

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5196`
- Swagger UI: `http://localhost:5196/swagger`

## API Endpoints

### POST /api/auth/register
Register a new user.

**Request Body:**
```json
{
  "username": "player1",
  "password": "password123",
  "role": "Player"
}
```

**Response:**
```json
{
  "message": "User registered successfully."
}
```

### POST /api/auth/login
Login and receive JWT token.

**Request Body:**
```json
{
  "username": "player1",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "player1",
  "role": "Player",
  "message": "Login successful."
}
```

### GET /api/auth/me
Get current user information (requires authentication).

**Headers:**
```
Authorization: Bearer <token>
```

**Response:**
```json
{
  "username": "player1",
  "role": "Player"
}
```

### GET /api/keys/keyshare/{roomId}
Get keyshare for an encrypted room (requires Admin role).

**Headers:**
```
Authorization: Bearer <token>
```

**Response:**
```json
{
  "keyshare": "a1b2c3d4e5f6g7h8",
  "message": "Keyshare retrieved successfully."
}
```

## Security Features

1. **Password Hashing**: SHA-256 hashing for password storage
2. **JWT Tokens**: Secure token-based authentication
3. **Account Lockout**: Protection against brute-force attacks
4. **Input Validation**: All inputs are validated
5. **Role-Based Access Control**: Admin-only endpoints

## Testing

You can test the API using:
- Swagger UI (available at `/swagger`)
- Postman or similar tools
- The Text Adventure client application

## Notes

- User data is stored in-memory (not persisted between restarts)
- JWT tokens expire after 24 hours
- Account lockout duration: 15 minutes
- Minimum password length: 6 characters
- Minimum username length: 3 characters

